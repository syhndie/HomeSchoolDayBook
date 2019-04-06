function displayStudentSubjectPairs() {
    var studentCheckboxes = document.getElementsByName("selectedStudents");
    var subjectCheckboxes = document.getElementsByName("selectedSubjects");

    var gradesHeader = document.getElementById("grades-header");
    while (gradesHeader.hasChildNodes()) {
        gradesHeader.removeChild(gradesHeader.lastChild);
    }

    var gradesContainer = document.getElementById("grades-container");
    while (gradesContainer.hasChildNodes()) {
        gradesContainer.removeChild(gradesContainer.lastChild);
    }

    var showHeader = false;

    for (var i = 0; i < studentCheckboxes.length; i++) {
        for (var j = 0; j < subjectCheckboxes.length; j++) {
            if (studentCheckboxes[i].checked === true && subjectCheckboxes[j].checked === true) {
                showHeader = true;

                var singleGradeRow = document.createElement("div");
                singleGradeRow.className = "form-group row";

                var studentCaptionCol = document.createElement("div");
                studentCaptionCol.className = "col-xs-3 col-md-2";

                var studentCaption = document.createElement("label");
                studentCaption.innerText = studentCheckboxes[i].dataset.studentname;
                studentCaption.className = "form-control-static notbold";

                studentCaptionCol.appendChild(studentCaption);

                var subjectCaptionCol = document.createElement("div");
                subjectCaptionCol.className = "col-xs-3 col-md-2";

                var subjectCaption = document.createElement("label");
                subjectCaption.innerText = subjectCheckboxes[j].dataset.subjectname;
                subjectCaption.className = "form-control-static notbold";

                subjectCaptionCol.appendChild(subjectCaption);

                var pointsEarnedCol = document.createElement("div");
                pointsEarnedCol.className = "col-xs-3 col-sm-2 input-column";

                var pointsEarnedTextbox = document.createElement("input");
                pointsEarnedTextbox.type = "text";
                pointsEarnedTextbox.name = "earned-student-" + studentCheckboxes[i].value + "-subject-" + subjectCheckboxes[j].value;
                pointsEarnedTextbox.className = "form-control input-grade";

                pointsEarnedCol.appendChild(pointsEarnedTextbox);

                var pointsAvailableCol = document.createElement("div");
                pointsAvailableCol.className = "col-xs-3 col-sm-2 input-column";

                var pointsAvailableTextbox = document.createElement("input");
                pointsAvailableTextbox.type = "text";
                pointsAvailableTextbox.name = "available-student-" + studentCheckboxes[i].value + "-subject-" + subjectCheckboxes[j].value;
                pointsAvailableTextbox.className = "form-control input-grade";

                pointsAvailableCol.appendChild(pointsAvailableTextbox);

                singleGradeRow.appendChild(studentCaptionCol);
                singleGradeRow.appendChild(subjectCaptionCol);
                singleGradeRow.appendChild(pointsEarnedCol);
                singleGradeRow.appendChild(pointsAvailableCol);

                gradesContainer.appendChild(singleGradeRow);
            }
        }
    }
    if (showHeader === true) {
        var pointsEarnedHeader = document.createElement("div");
        pointsEarnedHeader.className = "text-center col-xs-offset-6 col-md-offset-4 col-xs-3 col-sm-2";
        pointsEarnedHeader.innerText = "Points Earned";

        var pointsAvailableHeader = document.createElement("div");
        pointsAvailableHeader.className = "text-center col-xs-3 col-sm-2";
        pointsAvailableHeader.innerText = "Points Available";

        gradesHeader.appendChild(pointsEarnedHeader);
        gradesHeader.appendChild(pointsAvailableHeader);
    } else {
        var noGradesDiv = document.createElement("div");
        noGradesDiv.className = "alert alert-info";

        var noGradesParagraph = document.createElement("p");
        noGradesParagraph.innerText = "Grades for each student can be assigned for each subject. " +
            "Select at least one student and at least one subject to enter grades.";

        noGradesDiv.appendChild(noGradesParagraph);
        gradesHeader.appendChild(noGradesDiv);
    }
}

function saveEntry() {
    var form = document.getElementById('entry-form');
    var $form = $(form);
    var validGrades = validateGrades();
    var validForm = $form.valid();
    if (validForm && validGrades) {
        $form.submit();
    }
}

function validateGrades() {
    var numbersExpression = /^[0-9]*$/;
    var grades = document.getElementsByClassName('input-grade');
    var gradesAreValid = true;
    for (var i = 0; i < grades.length; i++) {
        var gradeColumn = grades[i].parentElement;
        while (grades[i].nextSibling) {
            gradeColumn.removeChild(gradeColumn.lastChild);
        }
        var gradeValue = grades[i].value;
        if (gradeValue !== '' && !gradeValue.match(numbersExpression)) {
            gradesAreValid = false;
            var validationMessage = document.createElement('p');
            validationMessage.innerText = "Points must be entered as integers (no decimals), or left blank.";
            validationMessage.style.color = '#a94442';
            gradeColumn.appendChild(validationMessage);            
        }
    }
    return gradesAreValid;
}