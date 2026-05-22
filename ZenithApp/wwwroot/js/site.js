function scrollToggle() {
    const btn = document.querySelector('.btn-scroll-bottom');
    const flipped = btn.classList.toggle('flipped');

    if (flipped) {
        window.scrollTo({ top: document.body.scrollHeight, behavior: 'smooth' });
    } else {
        window.scrollTo({ top: 0, behavior: 'smooth' });
    }
}

 function aceitarTermos(event) {

        event.preventDefault();

        localStorage.setItem(
            "termosAceitos",
            "true"
        );

        sessionStorage.setItem(
            "voltouDosTermos",
            "true"
        );

        window.location.href = "/Auth/Cadastro";
}