var exampleModal = document.getElementById('exampleModal')
exampleModal.addEventListener('show.bs.modal', function (event) {
    // Button that triggered the modal
    var button = event.relatedTarget

    var id = button.getAttribute('data-bs-course_id')
    var modalBodyInputCourseId = exampleModal.querySelector('.modal-body #courseId')
    modalBodyInputCourseId.value = id

    var recipient = button.getAttribute('data-bs-name')
    var modalTitle = exampleModal.querySelector('.modal-title')
    var modalBodyInputName = exampleModal.querySelector('.modal-body #courseTitle')
    modalTitle.textContent = "Изменения для курса " + recipient
    modalBodyInputName.value = recipient

    var description = button.getAttribute('data-bs-description')
    var modalBodyInputDescription = exampleModal.querySelector('.modal-body #courseDescription')
    modalBodyInputDescription.value = description})