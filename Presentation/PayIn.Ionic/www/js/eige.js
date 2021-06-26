angular
.module('app')
.filter('eigeTipoValidacionSentidoFilter', function () {
    return function (input) {
        var result =
            input===0 ? 'E' :
            input===1 ? 'S' :
            '';
        return result;
    };
})
.filter('eigeTipoValidacionTransporteFilter', function () {
    return function (input) {
        var result =
            input===0 ? 'B' :
            input===1 ? 'F' :
            '';
        return result;
    };
})
.filter('eigeTipoValidacionTransbordoFilter', function () {
    return function (input) {
        var result =
            input===0 ? 'N' :
            input===1 ? 'T' :
            '';
        return result;
    };
})
.filter('eigeZonaHistoricoFilter', function () {
    return function (input) {
        var result =
            input===0 ? 'A' :
            input===1 ? 'B' :
            input===2 ? 'C' :
            input===3 ? 'D' :
            '';
        return result;
    };
})
.filter('eigeZonaFilter', function () {
    return function (input) {
        var result =
            (input & 0x01 ? 'A' : '') +
            (input & 0x02 ? 'B' : '') +
            (input & 0x04 ? 'C' : '') +
            (input & 0x08 ? 'D' : '');
        return result;
    };
})
.filter('eigeUid', function () {
    // function format (number, n, x, s, c) {
    //     var re = '\\d(?=(\\d{' + (x || 3) + '})+' + (n > 0 ? '\\D' : '$') + ')';
    //     var num = ("000" + number.toFixed(Math.max(0, ~~n))).slice(-12);

    //     return (c ? num.replace('.', c) : num).replace(new RegExp(re, 'g'), '$&' + (s || ','));
    // };
    return function (input) {
        if (!input)
            return '';
            
        var inputInteger =
            (
                ((parseInt(input.substring(6, 8), 16) * 256 +
                parseInt(input.substring(4, 6), 16)) * 256 +
                parseInt(input.substring(2, 4), 16)) * 256 +
                parseInt(input.substring(0, 2), 16)
            ) * 100 +
            (
                parseInt(input.substring(0, 2), 16) ^
                parseInt(input.substring(2, 4), 16) ^
                parseInt(input.substring(4, 6), 16) ^
                parseInt(input.substring(6, 8), 16)
            ) % 100;
        
        var result = ("000" + inputInteger.toString()).slice(-12)
        result =
            result.slice(0, 4) + " " +
            result.slice(4, 8) + " " +
            result.slice(8, 12);
             //format(inputInteger, 0, 4, ' ');
        return result;
    };
})
;