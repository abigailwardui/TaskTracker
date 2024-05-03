function DeleteTask(taskId) {
    $.ajax({
        url: '/List/DeleteTask', // URL to target the DeleteTask action in ListController
        type: 'POST',
        data: {
            id: taskId // Sending taskId as the parameter
        },
        success: function () {
            window.location.reload(); // Reload the page after successful deletion
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
            // Check if the operation was successful
            if (response.success) {
                // Reload the current page to refresh the view
                location.reload();
            } else {
                // Handle error
                console.error("Error adding task");
            }
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
}

// Event listener for the "Add Task" button
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
            // Check if the operation was successful
            if (response.success) {
                // Reload the current page to refresh the view
                location.reload();
            } else {
                // Handle error
                console.error("Error adding task");
            }
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
}

// Event listener for the "Add List" button
$(document).ready(function () {
    $('#addListButton').click(function () {
        addList();
    });
});