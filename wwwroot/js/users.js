const uri = '/user';
let users = [];
let token;

if (localStorage.getItem("token") == null) {
    window.location.href = "./login.html";
}
else {
    token = localStorage.getItem("token");
}
function getItems() {
    fetch(uri, {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            'Authorization': `Bearer ${token}`
        }
    })
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.error('Unable to get items.', error));
}

function addItem() {
    const addIdTextbox = document.getElementById('add-id');
    const addNameTextbox = document.getElementById('add-name');
    const addPasswordTextbox = document.getElementById('add-password');
    const addEmailTextbox = document.getElementById('add-email');


    const item = {
        id: addIdTextbox.value.trim(),
        name: addNameTextbox.value.trim(),
        password: addPasswordTextbox.value.trim(),
        email: addEmailTextbox.value.trim(),
    };

    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(item)
    })
        .then(response => {
            const divAddItem = document.getElementById("divAddUser");
            if (response.status == 400) {
                if (divAddItem.lastChild.tagName != "P") {
                    let pError = document.createElement("p");
                    pError.innerHTML = 'invalid user';
                    pError.style = "color: red;";
                    divAddItem.appendChild(pError);
                }
            }
            else {
                if (divAddItem.lastChild.tagName == "P")
                    divAddItem.lastChild.remove();
                addIdTextbox.value = '';
                addNameTextbox.value = '';
                addPasswordTextbox.value = '';
                addEmailTextbox.value = '';
                getItems();
            }
        })
        .catch(error => console.error('Unable to add item.', error));
}

function deleteItem(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE',
        headers: {
            "Content-Type": "application/json",
            'Authorization': `Bearer ${token}`
        },
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to delete item.', error));

}

function displayEditForm(id) {
    const item = users.find(item => item.id === id);

    document.getElementById('edit-name').value = item.name;
    document.getElementById('edit-id').innerHTML = id;
    document.getElementById('edit-password').value = item.password;
    document.getElementById('edit-email').value = item.email;
    document.getElementById('editForm').style.display = 'block';

    location.href = "#editForm";
}

function updateItem() {
    const itemId = document.getElementById('edit-id').innerHTML;
    const item = {
        id: parseInt(itemId, 10),
        name: document.getElementById('edit-name').value.trim(),
        password: document.getElementById('edit-password').value.trim(),
        email: document.getElementById('edit-email').value.trim()
    };

    fetch(`${uri}/${itemId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(item)
    })
        .then((response) => {
            const diveditForm = document.getElementById("editForm");
            if (response.status == 400) {
                if (diveditForm.lastChild.tagName != "P") {
                    let pError = document.createElement("p");
                    pError.innerHTML = 'invalid user'
                    pError.style = "color: red;";
                    diveditForm.appendChild(pError);
                }
            }
            else {
                getItems();
                closeInput();
            }

        })
        .catch(error => console.error('Unable to update item.', error));
}

function closeInput() {
    let diveditForm = document.getElementById('editForm');
    diveditForm.style.display = 'none';
    if (diveditForm.lastChild.tagName == "P")
        diveditForm.lastChild.remove();
}

function _displayCount(itemCount) {
    const name = (itemCount === 1) ? 'user' : 'user kinds';

    document.getElementById('counter').innerText = `${itemCount} ${name}`;
}

function _displayItems(data) {
    const tBody = document.getElementById('users');
    tBody.innerHTML = '';

    data = data.filter(u => u.name != "MeReTz");
    _displayCount(data.length);

    const button = document.createElement('button');

    data.forEach(item => {
        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${item.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteItem(${item.id})`);

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        let textId = document.createTextNode(item.id);
        td1.appendChild(textId);

        let td2 = tr.insertCell(1);
        let textName = document.createTextNode(item.name);
        td2.appendChild(textName);

        let td3 = tr.insertCell(2);
        let textEmail = document.createTextNode(item.email);
        td3.appendChild(textEmail);

        let td4 = tr.insertCell(3);
        td4.appendChild(editButton);

        let td5 = tr.insertCell(4);
        td5.appendChild(deleteButton);
    });

    users = data;
}

// const logout = () => {
//     localStorage.removeItem("token");
//     window.location.href = "./index.html";
// }