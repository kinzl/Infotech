window.onload = function() {

    let numberOfGeneratedDiv = 1;
    let numberOfRadioButtons = 5;

    let lastDivTagValue = 0;

    for (let j = 0; j < numberOfGeneratedDiv; j++) {
        //DivContainer
        let div = document.createElement("div");
        div.id = "radioButtonContainer" + j;
        div.className = "radio_button_group";
        let marginTopVal = (j * 500);
        div.style.marginTop = marginTopVal + "px";
        document.body.appendChild(div);

        //Criticality
        let criticality = document.createElement("label");
        criticality.id = "criticality" + j;
        criticality.className = "criticality";
        criticality.textContent = "Criticality";
        document.getElementById("radioButtonContainer" + j).appendChild(criticality);

        //Question
        let question = document.createElement("label");
        question.id = "question" + j;
        question.className = "question";
        question.textContent = "Question XY";
        document.getElementById("radioButtonContainer" + j).appendChild(question);

        //Categorie
        let categorie = document.createElement("label");
        categorie.id = "categorie" + j;
        categorie.className = "categorie";
        categorie.textContent = "Categorie XY";
        document.getElementById("radioButtonContainer" + j).appendChild(categorie);

        //Radio Buttons
        for (let i = 0; i < numberOfRadioButtons; i++) {
            var radioButton1 = document.createElement("input");
            radioButton1.type = "radio";
            radioButton1.name = "radioGroup" + j;
            if (i === 0) {
                radioButton1.value = "  n.A.";
            } else {
                radioButton1.value = (i - 1) + "  Any text";
            }

            radioButton1.id = "radioButton" + (j + 1) + (i + 1);

            let label1 = document.createElement("label");
            label1.htmlFor = radioButton1.id;
            label1.innerHTML = radioButton1.value;

            document.getElementById("radioButtonContainer" + j).appendChild(radioButton1);
            document.getElementById("radioButtonContainer" + j).appendChild(label1);
            document.getElementById("radioButtonContainer" + j).appendChild(document.createElement("br"));
        }

        //TextArea
        let textArea = document.createElement("textarea");
        textArea.id = "textArea" + j;
        textArea.className = "textAreaGroup";
        document.getElementById("radioButtonContainer" + j).appendChild(textArea);

        lastDivTagValue = j;
    }
//nextButton
    let next = document.createElement("button");
    next.id = "buttonNext";
    next.className = "buttonNext";
    next.textContent = "Next";
// button.onClick = "RedirectToAnswerQuestionsExtended";
    document.getElementById("radioButtonContainer" + (lastDivTagValue)).appendChild(next);
    next.onclick = function () {
        location.href = "AnswerQuestionsExtended";
    }
}

function x() {
    console.log("x");
}