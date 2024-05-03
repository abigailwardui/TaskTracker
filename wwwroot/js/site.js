function DeleteTask(taskId) {
    $.ajax({
        url: '/List/DeleteTask',
        type: 'POST',
        data: {
            id: taskId
        },
        success: function () {
            window.location.reload();
        }
    });
}

function DeleteList(i) {
    $.ajax({
        url: 'Home/DeleteList',
        type: 'POST',
        data: {
            id: i
        },
        success: function () {
            window.location.reload();
        }
    });
}

function SetList(listId) {
    window.location.href = "/?id=" + listId;
};

function ToggleTaskCompletion(taskId, isChecked) {
    var label = $('label[for="task-' + taskId + '"]');

    if (isChecked) {
        label.addClass('completed-task');
    } else {
        label.removeClass('completed-task');
    }
    $.ajax({
        url: '/Home/ToggleCompletion',
        type: 'POST',
        data: { id: taskId, isComplete: isChecked },
        success: function (response) {
        },
        error: function (xhr, status, error) {
        }
    });
}

function addTask() {
    var formData = $('#addTaskForm').serialize();
    $.ajax({
        url: '/List/AddTask',
        type: 'POST',
        data: formData,
        success: function (response) {
            if (response.success) {
                location.reload();
            } else {
                console.error("Error adding task");
            }
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
}

$(document).ready(function () {
    $('#addTaskButton').click(function () {
        addTask();
    });
});

function addList() {
    var formData = $('#addListForm').serialize();
    $.ajax({
        url: '/Home/AddList',
        type: 'POST',
        data: formData,
        success: function (response) {
            if (response.success) {
                location.reload();
            } else {
                console.error("Error adding task");
            }
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
}

$(document).ready(function () {
    $('#addListButton').click(function () {
        addList();
    });
});