$('.main-slider').slick({
    dots: false,
    infinite: true,
    autoplay: false,
    speed: 300,
    slidesToShow: 1,
    slidesToScroll: 1,
    responsive: [
      {
        breakpoint: 1024,
        settings: {
          slidesToShow: 1,
          slidesToScroll: 1,
          infinite: true,
          dots: false
        }
      },
      {
        breakpoint: 600,
        settings: {
          slidesToShow: 1,
          slidesToScroll: 1
        }
      },
      {
        breakpoint: 480,
        settings: {
          slidesToShow: 1,
          slidesToScroll: 1
        }
      }
      // You can unslick at a given breakpoint now by adding:
      // settings: "unslick"
      // instead of a settings object
    ]
  });

  $(function(){
    // wow js
    new WOW().init();
    // date validation
    var d = new Date();
    var month = d.getMonth() + 1; // month
    var day = d.getDate(); //day
    var minDay = (month < 10 ? '0' : '') + month + '-' + (day < 10 ? '0' : '') + day + '-' + d.getFullYear();
    // var maxDay = d.getDate()+6;
    // var maxDate = d.getFullYear() + '-' + (month < 10 ? '0' : '') + month + '-' + (maxDay < 10 ? '0' : '') + maxDay;
    $("#demo").append(minDay);
    // let dateNow = Date.now();
    // $("#dateInput").attr("min",minDay);
    // $("#dateInput").attr("max",maxDate);
    // $("input[type='date']").attr("min",minDay);

    $(function () {
        $('#daterange').daterangepicker({
            minDate: minDay
        }, function (start, end, label) {
            console.log("A new date selection was made: " + start.format('YYYY-MM-DD') + ' to ' + end.format('YYYY-MM-DD'));
        });
    });


        })

let adult_number = document.getElementById('adult-number');
let adult_minus = document.getElementById('adultsubGuestNum');
let adult_plus = document.getElementById('adultaddGuestNum');
let child_number = document.getElementById('child-number');
let child_minus = document.getElementById('childsubGuestNum');
let child_plus = document.getElementById('childaddGuestNum');

adult_plus.onclick = function() {
   adult_number.innerHTML++;
}

adult_minus.onclick = function(){
    if (adult_number.innerHTML > 1) {
        adult_number.innerHTML--;
    }
    else{
        adult_number.innerHTML = 1
    }
}

child_plus.onclick = function() {
  child_number.innerHTML++;
}

child_minus.onclick = function(){
  if (child_number.innerHTML > 1) {
      child_number.innerHTML--;
  }
  else{
      child_number.innerHTML = 1
  }
}

document.addEventListener("click",(e)=>{
  if(!e.target.matches("#addGuestBtn")){
    document.querySelector(".guest-member-content").style.display="none"
  }
  if(e.target.matches("#addGuestBtn")){
    document.querySelector(".guest-member-content").style.display="flex"
  }
})