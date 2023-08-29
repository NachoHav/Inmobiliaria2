const dropdownItems = document.querySelectorAll('.dropdown-hover');

dropdownItems.forEach(item => {
    item.addEventListener('mouseenter', () => {
        item.querySelector('.dropdown-menu').classList.add('show');
    });

    item.addEventListener('mouseleave', () => {
        item.querySelector('.dropdown-menu').classList.remove('show');
    });
});

