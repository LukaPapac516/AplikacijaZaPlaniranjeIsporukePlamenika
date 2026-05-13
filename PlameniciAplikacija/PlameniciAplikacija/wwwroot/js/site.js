// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

(function () {
	const debounceTimers = new WeakMap();
	const activeRequests = new WeakMap();
	const debounceDelay = 250;

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
			return;
		}

		if (!control.matches('select, input[type="checkbox"], input[type="radio"]')) {
			return;
		}

		scheduleRefresh(form);
	});
})();
