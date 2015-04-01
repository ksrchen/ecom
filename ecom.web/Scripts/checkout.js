$(document).ready(function () {

    setDefaults();

   

  
    $("#completePurchase").click(function () {
       

        var contactInfo = {};
        $.each($('#contact_form').serializeArray(), function (_, kv) {
            contactInfo[kv.name] = kv.value;
        });
        var referenceNumber = new Date().getMilliseconds();
        var amount = new Number($("#productPrice").val());

        // Disable the submit button to prevent repeated clicks
        $("#completePurchase").prop('disabled', true);

        mpps.createPaymentToken(
            {
                billingContact: contactInfo,
                referenceNumber: referenceNumber,
                profile: 'LNDEV',
               // ptcProxyUrl: window.location.protocol + '//' +  window.location.host + '/ptcproxy.aspx'
            }).then( function (event) {
                $("#completePurchase").prop('disabled', false);
                if (event.response.Status) {
                    var msg = $("#message");
                    msg.empty();
                    msg.removeClass("message-error");
                    msg.addClass("message-success");
                    msg.append("Your payment has been accepted<br/>");
                    msg.append('Token:' + event.response.Token + '<br/>');
                }
                else {
                    var msg = $("#message");
                    msg.empty();
                    msg.removeClass("message-success");
                    msg.addClass("message-error");
                    //msg.append("Fail to process payment<br/>");
                    msg.append(event.response.Message + '<br/>');
                }
            }).fail(function (errors) {
                $("#completePurchase").prop('disabled', false);
                var msg = $("#message");
                msg.empty();
                msg.removeClass("message-success");
                msg.addClass("message-error");
                //msg.append("Fail to process payment<br/>");                

                $(errors).each(function (idx, el) {
                    msg.append(el + "<br/>");
                });
            });        
        return false;
    });
   
    mpps.buildPaymentForm("#mpps-payment").then(function () {
       // $("input[data-mpps='card_type']").val("001");
     //   $("input[data-mpps='card_number']").val("4242424242424242");
        $("input[data-mpps='card_expiry_date']").val("11-2020");
        $("input[data-mpps='card_cvn']").val("120");
    });

});


function setDefaults() {
    //$("input[name='transaction_type']").val("authorization");
    //$("input[name='reference_number']").val(new Date().getTime());
    //$("input[name='amount']").val("100.00");
    //$("input[name='currency']").val("USD");
    //$("input[name='payment_method']").val("card");
    $("input[name='firstName']").val("John");
    $("input[name='lastName']").val("Doe");
    $("input[name='email']").val("null@cybersource.com");
    $("input[name='phone']").val("02890888888");
    $("input[name='addressLine1']").val("1 Card Lane");
    $("input[name='city']").val("My City");
    $("input[name='state']").val("CA");
    $("input[name='country']").val("US");
    $("input[name='postalCode']").val("94043");
}