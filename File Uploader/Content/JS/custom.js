var selectFile = "Please select file!";
var largeFileSize = "File is larger, please select a file less than 10mb";
var message = "";
var hasUploaded = false;

var orderNbr = "";
var caseID = "";
var $loading = $('#loading');

var valuesList = [];
var Okay = true;

//submit button enable disable
function btnStatus(isEnabled) {
    $(".btn-lg").prop('disabled', isEnabled);
    if (isEnabled) {
        $("#txtreqnbr").addClass("error");
    }
}


function GetURLParameter(sParam) {
    var sPageURL = window.location.search.substring(1);

    var sURLVariables = sPageURL.split('&');
    for (var i = 0; i < sURLVariables.length; i++) {
        var sParameterName = sURLVariables[i].split('=');
        if (sParameterName[0] == sParam) {
            return sParameterName[1];
        }
    }
}




///control will be created and append to the column
function CreateControl(controlType, i, label, attributeID, isRequired) {
    var formOutlineDiv = $("<div class='form-outline'> </div>");
    var rowDiv = $("<div class='row'> </div>");
    var rowInnerDiv = $("<div class='col-md-12 col-sm-12 mb-4 '> </div>");

    var isRequiredControl = false;

    if (isRequired.toLowerCase().trim() === "true")
        isRequiredControl = true;

    //switch to get control type
    switch (controlType.toLowerCase()) {

        case "text":
            var text = $('<input>').attr({
                type: 'text',
                placeholder: $(label).text(),
                id: 'txtq' + (i + 1).toString(),
                name: 'txtq' + (i + 1).toString()
            }).addClass("form-control form-control-lg w-100").attr("data-attributeID", attributeID).prop("required", isRequiredControl);

            

            formOutlineDiv.append(label).append(text);
            rowInnerDiv.append(formOutlineDiv);
            rowDiv.append(rowInnerDiv);
            break;
        case "file":

            var infoDiv = $("<div class='form-label mt-3 ml-1 '></div>");
            var infoPara1 = $("<p class='fileinfo'>File size limit: 10MB </p>");
            var infoPara2 = $(" <p class='fileinfo'>Allowed file type : Word, Excel, PPT, PDF, Image, Video </p>");

            infoDiv.append(infoPara1).append(infoPara2);

            var text = $('<input>').attr({
                type: 'file',
                id: 'txtq' + (i + 1).toString(),
                name: 'txtq' + (i + 1).toString(),
            }).addClass("form-control form-control-lg w-100").attr("accept", ".als, .cer, .csv, .dat, .doc, .docx, .epl, .exe, .gif, .ico, .ics, .jpeg, .jpg, .js, .mdb, .msi, .ofx, .pdf, .pfx, .ppt, .pptx, .qbo, .qfx, .rar, .rtf, .sql, .swf, .txt, .xls, .xlsx, .xml, .zip, .zpl, .pbix, .png, .svg, .tif, .tiff")
                .attr("onchange", "Filevalidation()");

            formOutlineDiv.append(label).append(text);
            rowInnerDiv.append(formOutlineDiv).append(infoDiv);
            rowDiv.append(rowInnerDiv);
            break;

        case "date":
            var date = $('<input>').attr({
                type: 'date',
                id: 'q' + (i + 1).toString(),
                name: 'q' + (i + 1).toString()
            }).addClass("form-control form-control-lg w-100").attr("data-attributeID", attributeID).prop("required", isRequiredControl);

           

            formOutlineDiv.append(label).append(date);
            rowInnerDiv.append(formOutlineDiv);
            rowDiv.append(rowInnerDiv);
            break;
        case "radio":
            var radioRow = $("<div class='row align-items-center'></div>");
            var radioGroupHeader = $("<div class='form-outline'></div>");
            var radioCol3 = $("<div class='col-md-3 col-sm-3 text-center '></div>");
            var radioCol9 = $("<div class='col-md-9 col-sm-9 ps-2 mb-0'></div>");
            var ratingLabel = $("<label>Rating</label>");

            radioCol3.append(ratingLabel);


            var radioInnerRow = $("<div class='row'></div>");


            //prepairing radio control and labels
            for (var j = 0; j < 3; j++) {
                var radioInnerRowColumn = $("<div class='col-md-4 col-sm-4 mb-0 text-center '></div>");
                var radio = $('<input>').attr({
                    type: 'radio',
                    id: 'rdq' + i + (j + 1).toString(),
                    name: attributeID
                }).addClass("w-100 d-none").attr("data-attributeID", attributeID);

                radio.prop("required", isRequiredControl); //added required property

                var rdimg = $('<img>').attr({
                    id: 'img' + i + (j + 1).toString(),
                }).addClass("w-100 radioImage rounded-circle");
                //Regular shadow

               

                //setting up radio values and attributes
                if (j < 3) {
                    if (j == 0) {
                        
                        rdimg.attr("src", $("#poor").val());
                        var labelPoor = $(" <label class='w-100' for='rdq" + i + (j + 1) + "'></label>").append(rdimg);

                        radioInnerRowColumn.append(labelPoor);  //radio label
                        radio.attr("data-value", "poor");
                    }
                    if (j == 1) {
                        rdimg.attr("src", $("#average").val());
                        var labelAverage = $(" <label class='w-100' for='rdq" + i + (j + 1) + "'></label>").append(rdimg);;
                        radioInnerRowColumn.append(labelAverage);
                        radio.attr("data-value", "average");
                    }
                    if (j == 2) {
                        rdimg.attr("src", $("#good").val());
                        var labelGood = $(" <label class='w-100' for='rdq" + i + (j + 1) + "'></label>").append(rdimg);;
                        radioInnerRowColumn.append(labelGood);
                        radio.attr("data-value", "good");
                    }
                }

                radioInnerRowColumn.append(radio);
                radioInnerRow.append(radioInnerRowColumn);
            }

            radioGroupHeader.append(label);
            rowInnerDiv.append(radioGroupHeader);

            radioCol9.append(radioInnerRow);
            radioRow.append(radioCol3);
            radioRow.append(radioCol9);
            formOutlineDiv.css("background-color", "#e9ecef");
            formOutlineDiv.append(radioRow);
            rowInnerDiv.append(formOutlineDiv);
            rowDiv.append(rowInnerDiv);
            break;
        default:
    }
    return rowDiv;
}

$(document).on("click", "img", function () {
    $(this).addClass("radioEnabled").parent().parent().siblings().find("img").removeClass("radioEnabled");
});

$(document).on("click", "input[type='radio']", function () {
    var values = {
        attributeID: $(this).data("attributeid"),
        value: $(this).data("value")
    };

    var found = false;

    if (valuesList.length > 0) {
        for (var i = 0; i < valuesList.length; i++) {
            if (values.attributeID == valuesList[i].attributeID) {
                valuesList[i].value = values.value;
                found = true;
                break;
            }
        }
    }
    if (!found) {
        valuesList.push(values);
    }
});

$(document).on("click keyup focusout", "input[type='date']", function () {

    var values = {
        attributeID: $(this).data("attributeid"),
        value: $(this).val()
    };
    var found = false;


    if (valuesList.length > 0) {
        for (var i = 0; i < valuesList.length; i++) {
            if (values.attributeID === valuesList[i].attributeID) {
                valuesList[i].value = values.value;
                found = true;
                break;
            }
        }
    }

    if (!found) {
        valuesList.push(values);
    }
});






Filevalidation = () => {
    var id = $(document).find("input[type='file'").attr("id");
    const fi = document.getElementById(id);
    // Check if any file is selected.
    if (fi.files.length > 0) {
        for (var i = 0; i <= fi.files.length - 1; i++) {

            var fsize = fi.files.item(i).size;
            var file = Math.round((fsize / 1024));
            // The size of the file.
            if (file > 10240) {
                alert(largeFileSize);
                $('#' + id).val('');
            }
        }
    }
}




//Attach the event handler to any element
$(document)
    .ajaxStart(function () {
        //ajax request went so show the loading image
        $loading.removeClass("d-none");
    })
    .ajaxStop(function () {
        //got response so hide the loading image
        $loading.addClass("d-none");
        if (hasUploaded) {
            alert(message);
        }
    });


