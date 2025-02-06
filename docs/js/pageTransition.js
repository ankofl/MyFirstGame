const TRANSITION_DURATION = 300; // в миллисекундах

document.addEventListener('DOMContentLoaded', function () {
    const transitionLayer = document.getElementById('page-transition');

    // Анимация появления страницы
    setTimeout(() => {
        transitionLayer.style.opacity = '0';
        setTimeout(() => {
            transitionLayer.style.zIndex = '-1'; // Убираем слой за другие элементы, чтобы он не мешал взаимодействию
        }, TRANSITION_DURATION); // Ждем, пока анимация завершится
    }, 10); // Небольшая задержка для браузера

    // Обработка переходов
    const pageLinks = document.querySelectorAll('.page-link');
    pageLinks.forEach(link => {
        link.addEventListener('click', function (event) {
            event.preventDefault();
            transitionToPage(this.getAttribute('href'));
        });
    });
});

function transitionToPage(url) {
    const transitionLayer = document.getElementById('page-transition');
    transitionLayer.style.opacity = '1';
    transitionLayer.style.zIndex = '1000'; // Возвращаем слой поверх всех элементов
    setTimeout(() => {
        window.location.href = url;
    }, TRANSITION_DURATION); // Время анимации
}