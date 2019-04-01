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
                pointsEarnedCol.className = "col-xs-3 col-sm-2";

                var pointsEarnedTextbox = document.createElement("input");
                pointsEarnedTextbox.type = "text";
                pointsEarnedTextbox.name = "student" + studentCheckboxes[i].value + "subject" + subjectCheckboxes[j].value + "earned";
                pointsEarnedTextbox.className = "form-control";

                pointsEarnedCol.appendChild(pointsEarnedTextbox);

                var pointsTotalCol = document.createElement("div");
                pointsTotalCol.className = "col-xs-3 col-sm-2";

                var pointsTotalTextbox = document.createElement("input");
                pointsTotalTextbox.type = "text";
                pointsTotalTextbox.name = "student" + studentCheckboxes[i].value + "subject" + subjectCheckboxes[j].value + "total";
                pointsTotalTextbox.className = "form-control";

                pointsTotalCol.appendChild(pointsTotalTextbox);

                singleGradeRow.appendChild(studentCaptionCol);
                singleGradeRow.appendChild(subjectCaptionCol);
                singleGradeRow.appendChild(pointsEarnedCol);
                singleGradeRow.appendChild(pointsTotalCol);

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