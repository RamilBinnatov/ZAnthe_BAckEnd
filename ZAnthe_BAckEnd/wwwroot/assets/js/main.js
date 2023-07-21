window.onscroll = function() {myFunction(),scrollFunction()};

let header = document.getElementById("myHeader");

let sticky = header.offsetTop;

function myFunction() {
  if (window.scrollY > sticky) {
    header.classList.add("sticky")
  }      
  else{
    header.classList.remove("sticky")
  }
}

let mybutton = document.getElementById("myBtn");

function scrollFunction() {
  if (document.documentElement.scrollTop > 20) {
    mybutton.style.display = "block";
  } else {
    mybutton.style.display = "none";
  }
}

function topFunction() {
  document.documentElement.scrollTop = 0;
}

let menu = document.querySelector(".header-right i");
let sidebar = document.querySelector("#sidebar");
let span = document.querySelector("#sidebar span");

menu.onclick = () => {
    sidebar.style.display = "flex"
}

span.onclick = () => {
    sidebar.style.display = "none"
}  

 

 