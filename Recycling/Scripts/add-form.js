$(document).ready(function () {
    // Define Ajax Form Search Funtion
    var ajaxFormSubmit = function () {
        var $form = $(this);
        var options = {
            url: $form.attr("action"),
            type: $form.attr("method"),
            data: $form.serialize()
        };

        $.ajax(options).done(function (data) {
            var $target = $($form.attr("data-rp-target"));
            $target.html(data);
        });


    };

    // Define AutoComlete Function
    var createAutocomplete = function () {

        var $input = $(this);

        var options = {
            source: $input.attr("data-rp-autocomplete")
        };

        $input.autocomplete(options);
    };

    //// Define get pagedList url Function
    //var getPage = function () {
    //    var $page = $(this);

    //    var options = {
    //        url: $page.attr("href"),
    //        type: "get"
    //    };

    //    alert("it is called");

    //    $.ajax(options).done(function (data) {

    //        var target = $page.parent("div.pagedList").attr("data-rp-target");
    //        $(target).replaceWith(data);
    //    });
    //    return false;

    //}


    $("form[data-rp-ajax='true']").submit(ajaxFormSubmit);
    $("input[data-rp-autocomplete]").each(createAutocomplete);

    //$("container body-content").on("click", ".pagedList a", getPage);

    //Upload Image
    //$("#uploadEditorImage").change(function () {
    //    var data = new FormData();
    //    var files = $("#uploadEditorImage").get(0).files;
    //    if (files.length > 0) {
    //        data.append("HelpSectionImages", files[0]);
    //    }
    //    $.ajax({
    //        url: "~/Product/FileUpload",
    //        type: "POST",
    //        processData: false,
    //        contentType: false,
    //        data: data,
    //        success: function (response) {
    //            //code after success

    //        },
    //        error: function (er) {
    //            alert(er);
    //        }

    //    });
    //});

    // Input file button click event
    $(document).on('change', '.btn-file :file', function () {
        var input = $(this),
            numFiles = input.get(0).files ? input.get(0).files.length : 1,
            label = input.val().replace(/\\/g, '/').replace(/.*\//, '');
        input.trigger('fileselect', [numFiles, label]);
    });
    $('.btn-file :file').on('fileselect', function (event, numFiles, label) {

        var input = $(this).parents('.input-group').find(':text'),
            log = numFiles > 1 ? numFiles + ' files selected' : label;

        if (input.length) {
            input.val(log);
        } else {
            if (log) alert(log);
        }

    });


    //The page will show constituents forms based on how many c_number we select on the page.
    function displaycinfo() {
        var constituent = $('#cnumber');
        var cnum = constituent.val();
        var clist_rest = "<label>Constituents Name: </label><input type='text' name='cform[cname][]' size='15' maxlength='20'/> <br /> \
											<label>Constituent weight:</label> <input type='text' name='cform[pweight][]' size='15' maxlength='20'/>g <br />\
											<label>Type:</label> <input type='text' name='cform[Type][]' size='15' maxlength='20' /></p>\
											<label>classification:</label> \
													<select name='cform[classification][]'> \
														<option value='Green Cart'>Green Cart</option> \
														<option value='Blue Bag #1: Paper'>Blue Bag #1: Paper</option> \
														<option value='Blue Bag #2: Recyclables'>Blue Bag #2: Recyclables</option> \
														<option value='Clear Garbage Bag'>Clear Garbage Bag</option> \
													</select><br /> \
        									 </div>";

        var clist_rest = '\
            <div class="form-group">\
                <label class="control-label col-md-2" for="ConstituentName">Constituent Name</label>\
                <div class="col-md-10">\
                    <input class="form-control text-box single-line" id="ConstituentName" name="cform[cname][]" type="text" value="" />\
                    <span class="field-validation-valid text-danger" data-valmsg-for="ConstituentName" data-valmsg-replace="true"></span>\
                </div>\
            </div>\
            <div class="form-group">\
                <label class="control-label col-md-2" for="PartWeight">Part Weight</label>\
                <div class="col-md-10">\
                    <input class="form-control text-box single-line" id="PartWeight" name="cform[pweight][]" type="text" value="" />\
                    <span class="field-validation-valid text-danger" data-valmsg-for="PartWeight" data-valmsg-replace="true"></span>\
                </div>\
            </div>\
            <div class="form-group">\
                <label class="control-label col-md-2" for="Type">Type</label>\
                <div class="col-md-10">\
                    <input class="form-control text-box single-line" id="Type" name="cform[type][]" type="text" value="" />\
                    <span class="field-validation-valid text-danger" data-valmsg-for="Type" data-valmsg-replace="true"></span>\
                </div>\
            </div>\
            <div class="form-group">\
                <label class="control-label col-md-2" for="Recyclability">Recyclability</label>\
                <div class="col-md-10">\
                    <input class="form-control text-box single-line" id="Recyclability" name="cform[classification][]" type="text" value="" />\
                    <span class="field-validation-valid text-danger" data-valmsg-for="Recyclability" data-valmsg-replace="true"></span>\
                </div>\
            </div>\
        </div>';

        //Remove previously generated forms
        $("div[id^='clist']").remove();
        for (var i = 0; i < cnum; i++) {

            $('#Constituentlist').after(			//HTML code for constituents info
         "<div class=lform-group' id='clist" + i + "'>" + clist_rest);
        }
    }

    $('#cnumber').change(displaycinfo); //Change the number of forms.
});