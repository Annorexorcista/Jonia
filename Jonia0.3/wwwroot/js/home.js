window.addEventListener('scroll', function() {
    var nav = document.querySelector('.nav');
    
    if (window.scrollY > 590) {
        nav.style.display = 'none';
    } else {
        nav.style.display = 'block';
    }
});

window.addEventListener('scroll', function() {
    var nav = document.querySelector('.logo-container');
    var logo = document.querySelector('.fixed-logo'); 

    if (window.scrollY > 590) {
        nav.style.display = 'none';
        logo.classList.add('logo-container'); 
    } else {
        nav.style.display = 'block';
        logo.classList.remove('logo-container'); 
    }
});
