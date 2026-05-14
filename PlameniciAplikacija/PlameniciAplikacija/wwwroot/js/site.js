// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

(function () {
	const debounceTimers = new WeakMap();
	const activeRequests = new WeakMap();
	const debounceDelay = 250;
	const autocompleteBlurSelector = '.autocomplete-input';

	function pad(number) {
		return String(number).padStart(2, '0');
	}

	function isHrLocale() {
		return (navigator.language || '').toLowerCase().startsWith('hr');
	}

	function formatDateParts(date, hrLocale) {
		const day = pad(date.getDate());
		const month = pad(date.getMonth() + 1);
		const year = date.getFullYear();
		return hrLocale ? `${day}.${month}.${year}` : `${month}/${day}/${year}`;
	}

	function formatTimeParts(date, hrLocale) {
		const hours = date.getHours();
		const minutes = pad(date.getMinutes());
		if (hrLocale) {
			return `${pad(hours)}:${minutes}`;
		}

		const isPm = hours >= 12;
		const displayHours = hours % 12 || 12;
		return `${displayHours}:${minutes} ${isPm ? 'PM' : 'AM'}`;
	}

	function parseDateText(text, hrLocale) {
		const trimmed = text.trim();
		const match = hrLocale
			? trimmed.match(/^(\d{1,2})\.(\d{1,2})\.(\d{4})$/)
			: trimmed.match(/^(\d{1,2})\/(\d{1,2})\/(\d{4})$/);
		if (!match) {
			return null;
		}

		const month = hrLocale ? Number(match[2]) : Number(match[1]);
		const day = hrLocale ? Number(match[1]) : Number(match[2]);
		const year = Number(match[3]);
		return { year, month, day };
	}

	function parseTimeText(text, hrLocale) {
		const trimmed = text.trim();
		if (hrLocale) {
			const match = trimmed.match(/^(\d{1,2}):(\d{2})$/);
			if (!match) {
				return null;
			}

			return { hour: Number(match[1]), minute: Number(match[2]) };
		}

		const match = trimmed.match(/^(\d{1,2}):(\d{2})(?:\s*([AaPp][Mm]))?$/);
		if (!match) {
			return null;
		}

		let hour = Number(match[1]);
		const minute = Number(match[2]);
		const meridiem = (match[3] || '').toUpperCase();
		if (meridiem === 'PM' && hour < 12) {
			hour += 12;
		}
		if (meridiem === 'AM' && hour === 12) {
			hour = 0;
		}

		return { hour, minute };
	}

	function toIsoLocal(date) {
		return `${date.getFullYear()}-${pad(date.getMonth() + 1)}-${pad(date.getDate())}T${pad(date.getHours())}:${pad(date.getMinutes())}:${pad(date.getSeconds())}`;
	}

	function setDatetimeError(control, message) {
		const error = control.querySelector('.datetime-control-error');
		const dateInput = control.querySelector('.datetime-control-date');
		const timeInput = control.querySelector('.datetime-control-time');
		if (error) {
			error.textContent = message || '';
		}
		[dateInput, timeInput].forEach((input) => {
			if (!input) return;
			input.classList.toggle('input-validation-error', Boolean(message));
		});
	}

	function attachDatetimeControl(control) {
		const dateInput = control.querySelector('.datetime-control-date');
		const timeInput = control.querySelector('.datetime-control-time');
		const hidden = control.querySelector('.datetime-control-value');
		if (!dateInput || !timeInput || !hidden) {
			return;
		}

		const hrLocale = isHrLocale();
		dateInput.placeholder = hrLocale ? 'dd.MM.yyyy' : 'MM/DD/YYYY';
		timeInput.placeholder = hrLocale ? 'HH:mm' : 'h:mm AM/PM';

		const initialValue = hidden.value ? new Date(hidden.value) : null;
		if (initialValue && !Number.isNaN(initialValue.getTime())) {
			dateInput.value = formatDateParts(initialValue, hrLocale);
			timeInput.value = formatTimeParts(initialValue, hrLocale);
		}

		function sync() {
			const dateText = dateInput.value.trim();
			const timeText = timeInput.value.trim();
			const requiredMessage = hidden.dataset.valRequired || '';
			const invalidMessage = hidden.dataset.datetimeInvalidMessage || 'Unesite valjan datum i vrijeme.';

			if (!dateText && !timeText) {
				hidden.value = '';
				setDatetimeError(control, requiredMessage || '');
				if (window.jQuery && window.jQuery.validator) {
					window.jQuery(hidden).valid();
				}
				return;
			}

			if (!dateText || !timeText) {
				hidden.value = '';
				setDatetimeError(control, invalidMessage);
				if (window.jQuery && window.jQuery.validator) {
					window.jQuery(hidden).valid();
				}
				return;
			}

			const dateParts = parseDateText(dateText, hrLocale);
			const timeParts = parseTimeText(timeText, hrLocale);
			if (!dateParts || !timeParts) {
				hidden.value = '';
				setDatetimeError(control, invalidMessage);
				if (window.jQuery && window.jQuery.validator) {
					window.jQuery(hidden).valid();
				}
				return;
			}

			const parsedDate = new Date(dateParts.year, dateParts.month - 1, dateParts.day, timeParts.hour, timeParts.minute, 0, 0);
			if (Number.isNaN(parsedDate.getTime())) {
				hidden.value = '';
				setDatetimeError(control, invalidMessage);
				if (window.jQuery && window.jQuery.validator) {
					window.jQuery(hidden).valid();
				}
				return;
			}

			hidden.value = toIsoLocal(parsedDate);
			setDatetimeError(control, '');
			if (window.jQuery && window.jQuery.validator) {
				window.jQuery(hidden).valid();
			}
		}

		dateInput.addEventListener('blur', sync);
		timeInput.addEventListener('blur', sync);
		dateInput.addEventListener('change', sync);
		timeInput.addEventListener('change', sync);

		control.addEventListener('keydown', (event) => {
			if (event.key === 'Enter' && (event.target === dateInput || event.target === timeInput)) {
				sync();
			}
		});

		control.addEventListener('focusout', (event) => {
			if (event.target === dateInput || event.target === timeInput) {
				sync();
			}
		});

		sync();
	}

	function buildUrl(form) {
		const url = new URL(form.action || window.location.href, window.location.origin);
		const searchParams = new URLSearchParams();
		const formData = new FormData(form);

		for (const [key, value] of formData.entries()) {
			if (typeof value === 'string' && value.trim() === '') {
				continue;
			}

			searchParams.append(key, value);
		}

		url.search = searchParams.toString();
		return url.toString();
	}

	function setLoadingState(root, isLoading) {
		root.classList.toggle('is-loading', isLoading);
		root.setAttribute('aria-busy', isLoading ? 'true' : 'false');
	}

	function replaceRoot(html, currentRoot, requestUrl) {
		const parser = new DOMParser();
		const documentFragment = parser.parseFromString(html, 'text/html');
		const nextRoot = documentFragment.querySelector('[data-ajax-list-root]');

		if (!nextRoot) {
			window.location.assign(requestUrl);
			return;
		}

		nextRoot.classList.add('ajax-enter');
		currentRoot.outerHTML = nextRoot.outerHTML;
		window.history.replaceState({}, '', requestUrl);
	}

	async function refreshList(form) {
		const root = form.closest('[data-ajax-list-root]');
		if (!root) {
			form.submit();
			return;
		}

		const requestUrl = buildUrl(form);
		const previousRequest = activeRequests.get(form);
		if (previousRequest) {
			previousRequest.abort();
		}

		const controller = new AbortController();
		activeRequests.set(form, controller);
		setLoadingState(root, true);

		try {
			const response = await fetch(requestUrl, {
				method: 'GET',
				credentials: 'same-origin',
				headers: {
					'X-Requested-With': 'XMLHttpRequest'
				},
				signal: controller.signal
			});

			if (!response.ok) {
				throw new Error(`HTTP ${response.status}`);
			}

			const html = await response.text();
			replaceRoot(html, root, requestUrl);
		} catch (error) {
			if (error.name !== 'AbortError') {
				window.location.assign(requestUrl);
			}
		} finally {
			const currentRequest = activeRequests.get(form);
			if (currentRequest === controller) {
				activeRequests.delete(form);
			}
			setLoadingState(root, false);
		}
	}

	function scheduleRefresh(form) {
		const previousTimer = debounceTimers.get(form);
		if (previousTimer) {
			clearTimeout(previousTimer);
		}

		const timer = window.setTimeout(() => {
			refreshList(form);
		}, debounceDelay);

		debounceTimers.set(form, timer);
	}

	document.addEventListener('submit', (event) => {
		const form = event.target.closest('form[data-ajax-search-form]');
		if (!form) {
			return;
		}

		event.preventDefault();
		refreshList(form);
	});

	document.addEventListener('input', (event) => {
		const control = event.target;
		const form = control.closest('form[data-ajax-search-form][data-ajax-live="true"]');
		if (!form) {
			return;
		}

		if (!control.matches('input[type="search"], input[type="text"]')) {
			return;
		}

		scheduleRefresh(form);
	});

	document.addEventListener('change', (event) => {
		const control = event.target;
		const form = control.closest('form[data-ajax-search-form][data-ajax-live="true"]');
		if (!form) {
			const statusControl = control.closest('.js-project-status');
			if (!statusControl) {
				return;
			}

			const projectRow = statusControl.closest('.project-row');
			if (!projectRow) {
				return;
			}

			projectRow.classList.remove('is-updated');
			projectRow.classList.add('status-flash');
			statusControl.classList.add('status-changed');
			window.setTimeout(() => {
				projectRow.classList.remove('status-flash');
				statusControl.classList.remove('status-changed');
			}, 500);
			return;
		}

		if (!control.matches('select, input[type="checkbox"], input[type="radio"]')) {
			return;
		}

		scheduleRefresh(form);
	});

	document.addEventListener('submit', (event) => {
		const form = event.target.closest('form.js-project-status-form');
		if (!form) {
			return;
		}

		const projectRow = form.closest('.project-row');
		if (projectRow) {
			projectRow.classList.add('is-saving');
		}
	});

	document.addEventListener('click', (event) => {
		const detailsLink = event.target.closest('.js-open-details');
		if (detailsLink && detailsLink.href && event.button === 0 && !event.metaKey && !event.ctrlKey && !event.shiftKey && !event.altKey) {
			event.preventDefault();
			const root = detailsLink.closest('[data-ajax-list-root]');
			if (root) {
				root.classList.add('detail-nav-out');
			}
			window.setTimeout(() => {
				window.location.assign(detailsLink.href);
			}, 140);
			return;
		}

		const collapseToggle = event.target.closest('[data-bs-toggle="collapse"]');
		if (!collapseToggle) {
			return;
		}

		const targetSelector = collapseToggle.getAttribute('data-bs-target');
		if (!targetSelector) {
			return;
		}

		const collapseTarget = document.querySelector(targetSelector);
		if (collapseTarget) {
			collapseTarget.classList.add('detail-expand-pulse');
			window.setTimeout(() => collapseTarget.classList.remove('detail-expand-pulse'), 450);
		}
	});

	window.addEventListener('load', () => {
		if (!window.jQuery || !window.jQuery.validator) {
			return;
		}

		window.jQuery.validator.setDefaults({
			onfocusout: function (element) {
				this.element(element);
			}
		});

		document.querySelectorAll('form').forEach((form) => {
			const validator = window.jQuery(form).data('validator');
			if (validator) {
				validator.settings.ignore = ':hidden:not(.autocomplete-value):not(.datetime-control-value)';
			}
		});

		document.addEventListener('blur', (event) => {
			const input = event.target;
			if (!input.matches(autocompleteBlurSelector)) {
				return;
			}

			const autocomplete = input.closest('.autocomplete');
			if (!autocomplete) {
				return;
			}

			const hidden = autocomplete.querySelector('.autocomplete-value');
			if (hidden) {
				window.jQuery(hidden).valid();
			}
		}, true);

		document.querySelectorAll('[data-datetime-control]').forEach(attachDatetimeControl);
	});
})();
