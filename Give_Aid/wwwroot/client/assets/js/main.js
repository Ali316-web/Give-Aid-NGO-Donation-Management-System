/* =========================================================================
   GIVE-AID — public site behaviours
   In the ASP.NET MVC build this file stays as-is (site.js under wwwroot/js).
   ========================================================================= */
document.addEventListener('DOMContentLoaded', () => {

  /* ---- Mobile nav toggle ---- */
  const navToggle = document.querySelector('[data-nav-toggle]');
  const mobileNav = document.querySelector('[data-mobile-nav]');
  if (navToggle && mobileNav) {
    navToggle.addEventListener('click', () => {
      mobileNav.classList.toggle('open');
      const isOpen = mobileNav.classList.contains('open');
      navToggle.innerHTML = isOpen ? '<i class="fa-solid fa-xmark"></i>' : '<i class="fa-solid fa-bars"></i>';
    });
  }

  /* ---- Accordion (Help Centre FAQs) ---- */
  document.querySelectorAll('.accordion-head').forEach(head => {
    head.addEventListener('click', () => {
      head.parentElement.classList.toggle('open');
    });
  });

  /* ---- Gallery filter chips ---- */
  document.querySelectorAll('[data-filter-group]').forEach(group => {
    const chips = group.querySelectorAll('.chip');
    chips.forEach(chip => {
      chip.addEventListener('click', () => {
        chips.forEach(c => c.classList.remove('active'));
        chip.classList.add('active');
        const filter = chip.dataset.filter;
        document.querySelectorAll('[data-gallery-item]').forEach(item => {
          const match = filter === 'all' || item.dataset.category === filter;
          item.style.display = match ? '' : 'none';
        });
      });
    });
  });

  /* ---- Donation amount chips ---- */
  document.querySelectorAll('.amount-chip-row').forEach(row => {
    const chips = row.querySelectorAll('.amount-chip[data-amount]');
    const customInput = document.querySelector('[data-custom-amount]');
    chips.forEach(chip => {
      chip.addEventListener('click', () => {
        chips.forEach(c => c.classList.remove('active'));
        chip.classList.add('active');
        if (customInput) customInput.value = chip.dataset.amount;
      });
    });
    if (customInput) {
      customInput.addEventListener('input', () => chips.forEach(c => c.classList.remove('active')));
    }
  });

  /* ---- Cause select pills ---- */
  document.querySelectorAll('.cause-select-row').forEach(row => {
    row.querySelectorAll('.cause-pill').forEach(pill => {
      pill.addEventListener('click', () => {
        row.querySelectorAll('.cause-pill').forEach(p => p.classList.remove('active'));
        pill.classList.add('active');
      });
    });
  });

  /* ---- Live payment card preview ---- */
  const cardNumInput = document.querySelector('#cardNumber');
  const cardNamePreview = document.querySelector('[data-preview-name]');
  const cardNumPreview = document.querySelector('[data-preview-number]');
  const cardExpPreview = document.querySelector('[data-preview-exp]');
  document.querySelector('#cardHolder')?.addEventListener('input', e => {
    if (cardNamePreview) cardNamePreview.textContent = e.target.value || 'YOUR NAME';
  });
  cardNumInput?.addEventListener('input', e => {
    let v = e.target.value.replace(/\D/g, '').slice(0, 16).replace(/(.{4})/g, '$1 ').trim();
    e.target.value = v;
    if (cardNumPreview) cardNumPreview.textContent = v || '•••• •••• •••• ••••';
  });
  document.querySelector('#cardExpiry')?.addEventListener('input', e => {
    let v = e.target.value.replace(/\D/g, '').slice(0, 4);
    if (v.length > 2) v = v.slice(0, 2) + ' / ' + v.slice(2);
    e.target.value = v;
    if (cardExpPreview) cardExpPreview.textContent = v || 'MM / YY';
  });

  /* ---- Simple client-side validation demo ---- */
  document.querySelectorAll('form[data-validate]').forEach(form => {
    form.addEventListener('submit', e => {
      e.preventDefault();
      let valid = true;
      form.querySelectorAll('[required]').forEach(input => {
        const field = input.closest('.field');
        if (!field) return;
        if (!input.value.trim() || (input.type === 'checkbox' && !input.checked)) {
          field.classList.add('is-invalid');
          field.classList.remove('is-valid');
          valid = false;
        } else {
          field.classList.remove('is-invalid');
          field.classList.add('is-valid');
        }
      });
      // password confirm match demo
      const pw = form.querySelector('#password');
      const cpw = form.querySelector('#confirmPassword');
      if (pw && cpw) {
        const field = cpw.closest('.field');
        if (pw.value !== cpw.value) { field.classList.add('is-invalid'); valid = false; }
      }
      if (valid) {
        const btn = form.querySelector('button[type="submit"]');
        if (btn) {
          const original = btn.innerHTML;
          btn.classList.add('is-loading');
          btn.innerHTML = '<span class="spinner" style="border-top-color:#fff"></span> Processing…';
          setTimeout(() => {
            const successUrl = form.dataset.successUrl;
            if (successUrl) { window.location.href = successUrl; }
            else { btn.classList.remove('is-loading'); btn.innerHTML = original; }
          }, 900);
        }
      } else {
        form.querySelector('.is-invalid')?.scrollIntoView({ behavior: 'smooth', block: 'center' });
      }
    });
  });

  /* ---- Modal open/close (delete confirmation, logout, etc.) ---- */
  document.querySelectorAll('[data-modal-target]').forEach(trigger => {
    trigger.addEventListener('click', () => {
      document.querySelector(trigger.dataset.modalTarget)?.classList.add('open');
    });
  });
  document.querySelectorAll('[data-modal-close]').forEach(closeBtn => {
    closeBtn.addEventListener('click', () => {
      closeBtn.closest('.modal-backdrop')?.classList.remove('open');
    });
  });
  document.querySelectorAll('.modal-backdrop').forEach(backdrop => {
    backdrop.addEventListener('click', e => { if (e.target === backdrop) backdrop.classList.remove('open'); });
  });

  /* ---- Alert dismiss ---- */
  document.querySelectorAll('.alert .close').forEach(btn => {
    btn.addEventListener('click', () => btn.closest('.alert').remove());
  });

});
