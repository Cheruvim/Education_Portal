var courseEditModel = document.getElementById('courseEditModel')
courseEditModel.addEventListener('show.bs.modal', function (event) {
    // Button that triggered the modal
    var button = event.relatedTarget

    var id = button.getAttribute('data-bs-course_id')
    var modalBodyInputCourseId = courseEditModel.querySelector('.modal-body #courseId')
    modalBodyInputCourseId.value = id

    var recipient = button.getAttribute('data-bs-name')
    var modalTitle = courseEditModel.querySelector('.modal-title')
    var modalBodyInputName = courseEditModel.querySelector('.modal-body #courseTitle')
    modalTitle.textContent = "Изменения для курса " + recipient
    modalBodyInputName.value = recipient

    var description = button.getAttribute('data-bs-description')
    var modalBodyInputDescription = courseEditModel.querySelector('.modal-body #courseDescription')
    modalBodyInputDescription.value = description})