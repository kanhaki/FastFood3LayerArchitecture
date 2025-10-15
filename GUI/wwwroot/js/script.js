'use strict';

function initHeaderInteractions() {
  const navbar = document.querySelector("[data-navbar]");
  const navbarLinks = document.querySelectorAll("[data-nav-link]");
  const menuToggleBtn = document.querySelector("[data-menu-toggle-btn]");
  const header = document.querySelector("[data-header]");
  const backTopBtn = document.querySelector("[data-back-top-btn]");
  const searchBtn = document.querySelector("[data-search-btn]");
  const searchContainer = document.querySelector("[data-search-container]");
  const searchSubmitBtn = document.querySelector("[data-search-submit-btn]");
  const searchCloseBtn = document.querySelector("[data-search-close-btn]");

  // Navbar toggle
  menuToggleBtn?.addEventListener("click", function () {
    navbar.classList.toggle("active");
    this.classList.toggle("active");
  });

  navbarLinks.forEach(link => {
    link.addEventListener("click", function () {
      navbar.classList.toggle("active");
      menuToggleBtn.classList.toggle("active");
    });
  });

  // Header sticky & back to top
  window.addEventListener("scroll", function () {
    if (window.scrollY >= 100) {
      header?.classList.add("active");
      backTopBtn?.classList.add("active");
    } else {
      header?.classList.remove("active");
      backTopBtn?.classList.remove("active");
    }
  });

  // Search box toggle
  [searchBtn, searchSubmitBtn, searchCloseBtn].forEach(btn => {
    btn?.addEventListener("click", function () {
      searchContainer?.classList.toggle("active");
      document.body.classList.toggle("active");
    });
  });
}

async function loadHTML(id, url) {
  const res = await fetch(url);
  const html = await res.text();
  document.getElementById(id).innerHTML = html;
  if (id === "header") {
    initHeaderInteractions(); // Chạy tương tác sau khi header load
  }
}

// Load header & footer
loadHTML("header", "partials/header.html");
loadHTML("footer", "partials/footer.html");
