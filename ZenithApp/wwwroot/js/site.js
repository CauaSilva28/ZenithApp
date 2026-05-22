function aceitarTermos() {
    localStorage.setItem("termosAceitos", "true");
}

function scrollToggle() {
    const btn = document.querySelector('.btn-scroll-bottom');
    const flipped = btn.classList.toggle('flipped');

    if (flipped) {
        window.scrollTo({ top: document.body.scrollHeight, behavior: 'smooth' });
    } else {
        window.scrollTo({ top: 0, behavior: 'smooth' });
    }
}
