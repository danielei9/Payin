var qrcode = new QRCode("qrcode", {
    text: "Pay[in]/ticket:{\"id\":1029}",
    width: 128,
    height: 128,
    colorDark : "#000000",
    colorLight : "#ffffff",
    correctLevel : QRCode.CorrectLevel.H
});

function makeCode () { 
    qrcode.makeCode("Pay[in]n/ticket:{\"id\":1029}");
}