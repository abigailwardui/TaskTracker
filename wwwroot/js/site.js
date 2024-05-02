function DeleteTask(i) {
    $.ajax({
        url: 'Home/DeleteTask',
        type: 'POST',
        data: {
            id: i
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
