const dropdownItems = document.querySelectorAll('.dropdown-hover');

dropdownItems.forEach(item => {
    let timeout;

    item.addEventListener('mouseenter', () => {
        clearTimeout(timeout);
        item.querySelector('.dropdown-menu').classList.add('show');
    });

    item.addEventListener('mouseleave', () => {
        timeout = setTimeout(() => {
            item.querySelector('.dropdown-menu').classList.remove('show');
        }, 300); // Añade una demora de 300 milisegundos (ajusta según tus preferencias)
    });
});
