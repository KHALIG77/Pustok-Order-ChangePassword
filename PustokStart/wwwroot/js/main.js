
//window.addEventListener("load", () => {
//    fetch("/book/addtobasket/")
//        .then(responce => responce.json())
//        .then(data => {
//            $(".text-number").text(data.length)
//        })

//})


let modalBtn = document.querySelectorAll(".modal-btn")
modalBtn.forEach((btn) => {
    btn.addEventListener("click", (e) => {
        e.preventDefault()
       
        let url = btn.getAttribute("href")

        fetch(url)
            .then(responce => responce.text())
            .then(data => {
                $("#quickModal .modal-dialog").html(data)
                $("#quickModal").modal("show")
            })
    })
})
let basketBtn = document.querySelectorAll(".hover-btns .basket-btn")
basketBtn.forEach((btn) => {
    btn.addEventListener("click", (e) => {
        e.preventDefault();
        let url = btn.getAttribute("href");
        fetch(url).then(response =>
            response.text())
            .then(data => {
               
                  $(".cart-block").html(data)
                 
            }
        )

    })
})



$(document).on("click", ".removefrombasket", function (e) {
    e.preventDefault();
    let url = $(this).attr("href");
    fetch(url)
        .then(response => {
            if (!response.ok) {
                alert("xeta bas verdi")
                return
            }
            return response.text()
        })
        .then(data => {
            $(".cart-block").html(data)
        })
})




