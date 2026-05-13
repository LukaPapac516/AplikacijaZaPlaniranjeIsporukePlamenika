document.addEventListener("DOMContentLoaded", function () {
    function debounce(fn, wait) {
        let t;
        return function (...args) {
            clearTimeout(t);
            t = setTimeout(() => fn.apply(this, args), wait);
        };
    }

    function attach(el) {
        const input = el.querySelector('.autocomplete-input');
        const hidden = el.querySelector('input[type="hidden"]');
        const list = el.querySelector('.autocomplete-list');
        const api = el.dataset.autocompleteApi;
        if (!input || !hidden || !list || !api) return;

        let controller = null;
        let items = [];
        let focused = -1;

        function clearList() {
            list.innerHTML = '';
            list.classList.add('d-none');
            list.setAttribute('aria-hidden', 'true');
            focused = -1;
            items = [];
        }

        function render(results) {
            clearList();
            if (!results || results.length === 0) return;
            results.forEach((r, idx) => {
                const it = document.createElement('div');
                it.className = 'autocomplete-item';
                it.setAttribute('role', 'option');
                it.dataset.value = r.id ?? r.text ?? r;
                it.textContent = r.text ?? r.id ?? r;
                it.addEventListener('click', () => select(idx));
                list.appendChild(it);
                items.push(it);
            });
            list.classList.remove('d-none');
            list.setAttribute('aria-hidden', 'false');
        }

        function select(idx) {
            if (idx < 0 || idx >= items.length) return;
            const it = items[idx];
            const v = it.dataset.value;
            input.value = it.textContent;
            hidden.value = v;
            clearList();
            input.focus();
        }

        const fetchSuggestions = debounce(async function () {
            const q = input.value.trim();
            if (controller) controller.abort();
            controller = new AbortController();
            try {
                const url = api + '?q=' + encodeURIComponent(q);
                const res = await fetch(url, { signal: controller.signal, headers: { 'X-Requested-With': 'XMLHttpRequest' } });
                if (!res.ok) { clearList(); return; }
                const data = await res.json();
                render(data);
            } catch (e) {
                if (e.name === 'AbortError') return;
                clearList();
            }
        }, 250);

        input.addEventListener('input', function () {
            hidden.value = '';
            fetchSuggestions();
        });

        input.addEventListener('keydown', function (ev) {
            if (ev.key === 'ArrowDown') {
                ev.preventDefault();
                if (items.length === 0) return;
                focused = Math.min(items.length - 1, focused + 1);
                items.forEach(i => i.classList.remove('focused'));
                items[focused].classList.add('focused');
                items[focused].scrollIntoView({ block: 'nearest' });
            } else if (ev.key === 'ArrowUp') {
                ev.preventDefault();
                if (items.length === 0) return;
                focused = Math.max(0, focused - 1);
                items.forEach(i => i.classList.remove('focused'));
                items[focused].classList.add('focused');
                items[focused].scrollIntoView({ block: 'nearest' });
            } else if (ev.key === 'Enter') {
                if (focused >= 0 && items[focused]) {
                    ev.preventDefault();
                    select(focused);
                }
            } else if (ev.key === 'Escape') {
                clearList();
            }
        });

        document.addEventListener('click', function (ev) {
            if (!el.contains(ev.target)) {
                clearList();
            }
        });
    }

    document.querySelectorAll('.autocomplete').forEach(attach);
});
