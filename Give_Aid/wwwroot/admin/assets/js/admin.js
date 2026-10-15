/* =========================================================================
   GIVE-AID — admin panel behaviours
   ========================================================================= */
document.addEventListener('DOMContentLoaded', () => {

  const shell = document.querySelector('.admin-shell');

  /* ---- Sidebar collapse (desktop) ---- */
  document.querySelector('[data-collapse-toggle]')?.addEventListener('click', () => {
    shell?.classList.toggle('collapsed');
  });

  /* ---- Sidebar open (mobile) ---- */
  document.querySelector('[data-sidebar-toggle]')?.addEventListener('click', () => {
    shell?.classList.toggle('sidebar-open');
  });
  document.querySelector('[data-sidebar-scrim]')?.addEventListener('click', () => {
    shell?.classList.remove('sidebar-open');
  });

  /* ---- Expandable submenus ---- */
  document.querySelectorAll('.side-group > .side-link[data-toggle-submenu]').forEach(link => {
    link.addEventListener('click', e => {
      e.preventDefault();
      link.closest('.side-group').classList.toggle('open');
      const sub = link.nextElementSibling;
      if (sub) sub.style.display = sub.style.display === 'flex' ? 'none' : 'flex';
    });
  });

  /* ---- Table select-all checkboxes ---- */
  document.querySelectorAll('[data-select-all]').forEach(master => {
    const table = master.closest('table');
    master.addEventListener('change', () => {
      table.querySelectorAll('tbody [data-row-check]').forEach(cb => cb.checked = master.checked);
    });
  });

  /* ---- Modal open/close (shared behaviour with public site) ---- */
  document.querySelectorAll('[data-modal-target]').forEach(trigger => {
    trigger.addEventListener('click', () => {
      document.querySelector(trigger.dataset.modalTarget)?.classList.add('open');
    });
  });
  document.querySelectorAll('[data-modal-close]').forEach(closeBtn => {
    closeBtn.addEventListener('click', () => closeBtn.closest('.modal-backdrop')?.classList.remove('open'));
  });
  document.querySelectorAll('.modal-backdrop').forEach(backdrop => {
    backdrop.addEventListener('click', e => { if (e.target === backdrop) backdrop.classList.remove('open'); });
  });

  /* ---- Notification dropdown ---- */
  const notifBtn = document.querySelector('[data-notif-toggle]');
  const notifPanel = document.querySelector('[data-notif-panel]');
  notifBtn?.addEventListener('click', () => notifPanel?.classList.toggle('open'));
  document.addEventListener('click', e => {
    if (notifPanel && !notifPanel.contains(e.target) && e.target !== notifBtn) notifPanel.classList.remove('open');
  });

  /* ---- Lightweight bar chart renderer (no external chart lib dependency) ----
     In the Razor build this would likely be swapped for Chart.js fed by a
     ViewModel; kept dependency-free here so the template runs standalone. */
  document.querySelectorAll('[data-bar-chart]').forEach(el => {
    const data = JSON.parse(el.dataset.barChart);
    const max = Math.max(...data.map(d => d.value));
    el.innerHTML = `<div style="display:flex;align-items:flex-end;gap:10px;height:180px;">
      ${data.map(d => `
        <div style="flex:1;display:flex;flex-direction:column;align-items:center;gap:8px;height:100%;justify-content:flex-end;">
          <div title="${d.label}: ${d.value}" style="width:100%;max-width:34px;border-radius:6px 6px 2px 2px;background:${d.color || 'var(--banyan-600)'};height:${(d.value/max*100).toFixed(0)}%;min-height:4px;"></div>
          <span style="font-size:11px;color:var(--ink-500);">${d.label}</span>
        </div>`).join('')}
    </div>`;
  });

  /* ---- Simple donut chart via conic-gradient ---- */
  document.querySelectorAll('[data-donut-chart]').forEach(el => {
    const data = JSON.parse(el.dataset.donutChart);
    const total = data.reduce((s, d) => s + d.value, 0);
    let acc = 0;
    const stops = data.map(d => {
      const start = (acc / total) * 360;
      acc += d.value;
      const end = (acc / total) * 360;
      return `${d.color} ${start}deg ${end}deg`;
    }).join(', ');
    el.innerHTML = `
      <div style="display:flex;align-items:center;gap:20px;">
        <div style="width:130px;height:130px;border-radius:50%;background:conic-gradient(${stops});flex-shrink:0;
             display:flex;align-items:center;justify-content:center;">
          <div style="width:76px;height:76px;border-radius:50%;background:var(--linen-50);display:flex;align-items:center;justify-content:center;font-size:12px;color:var(--ink-500);text-align:center;">Total<br><strong style="color:var(--ink-900);font-size:15px;">${total}</strong></div>
        </div>
        <div style="display:flex;flex-direction:column;gap:8px;">
          ${data.map(d => `<span style="display:flex;align-items:center;gap:8px;font-size:13px;color:var(--ink-700);">
            <span style="width:9px;height:9px;border-radius:50%;background:${d.color};display:inline-block;"></span>
            ${d.label} <strong style="margin-left:4px;">${Math.round(d.value/total*100)}%</strong>
          </span>`).join('')}
        </div>
      </div>`;
  });

});
