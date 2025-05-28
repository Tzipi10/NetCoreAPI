const uri = '/gift';
let gifts = [];
let token;
let currentUser;

const init = () => {
    if (localStorage.getItem("token") == null) {
        window.location.href = "./login.html";
    }
    else {
        token = localStorage.getItem("token");
        const payload = JSON.parse(atob(token.split('.')[1]));
        
        const currentUserId = payload["userId"];
        const role = payload["type"];
        if (role == "Admin") {
            const usersPageLink = document.getElementById('usersLink');
            usersPageLink.style.display = 'block';
        }

        const myName = document.getElementById('myName');
        getUser(currentUserId).then(user => {
            currentUser = user;
            myName.innerHTML = role == "Admin"? `${user.name} - Admin` : user.name;        
        });
    }
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
    const addNameTextbox = document.getElementById('add-name');
    const addPriceTextbox = document.getElementById('add-price');
    const addSummaryTextbox = document.getElementById('add-summary');

    const item = {
        name: addNameTextbox.value.trim(),
        price: addPriceTextbox.value.trim(),
        summary: addSummaryTextbox.value.trim(),
        id: 0
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
            const divAddItem = document.getElementById("divAddItem");
            if (response.status == 400) {
                if (divAddItem.lastChild.tagName != "P") {
                    let pError = document.createElement("p");
                    pError.innerHTML = 'invalid item'
                    pError.style = "color: red;";
                    divAddItem.appendChild(pError);
                }
            }
            else {
                if (divAddItem.lastChild.tagName == "P")
                    divAddItem.lastChild.remove();
                getItems();
                addNameTextbox.value = '';
                addPriceTextbox.value = '';
                addSummaryTextbox.value = '';
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
    const item = gifts.find(item => item.id === id);

    document.getElementById('edit-name').value = item.name;
    document.getElementById('edit-id').innerHTML = item.id;
    document.getElementById('edit-price').value = item.price;
    document.getElementById('edit-summary').value = item.summary;
    document.getElementById('editForm').style.display = 'block';

    window.location.href = "#editForm";
}

function updateItem() {
    const itemId = document.getElementById('edit-id').innerHTML;
    const item = {
        id: parseInt(itemId, 10),
        price: document.getElementById('edit-price').value.trim(),
        name: document.getElementById('edit-name').value.trim(),
        summary: document.getElementById('edit-summary').value.trim(),
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
                    pError.innerHTML = 'invalid item'
                    pError.style = "color: red;";
                    diveditForm.appendChild(pError);
                }
            }
            else {
                getItems()
                closeInput();
            }

        })
        .catch(error => console.error('Unable to update item.', error));



    return false;
}

function getUser(userId) {

    return fetch(`/user/${userId}`, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        }
    })
        .then(response => response.json())
}

function closeInput() {
    let diveditForm = document.getElementById('editForm');
    if (diveditForm.lastChild.tagName == "P")
        diveditForm.lastChild.remove();
    diveditForm.style.display = 'none';
}

function _displayCount(itemCount) {
    const name = (itemCount === 1) ? 'gift' : 'gift kinds';

    document.getElementById('counter').innerText = `${itemCount} ${name}`;
}

function _displayItems(data) {
    const tBody = document.getElementById('gifts');
    tBody.innerHTML = '';

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

        let td7 = tr.insertCell(0);
        let textId = document.createTextNode(item.id);
        td7.appendChild(textId);

        let td1 = tr.insertCell(1);
        let textName = document.createTextNode(item.name);
        td1.appendChild(textName);

        let td2 = tr.insertCell(2);
        let textPrice = document.createTextNode(item.price);
        td2.appendChild(textPrice);

        let td3 = tr.insertCell(3);
        let textsum = document.createTextNode(item.summary);
        td3.appendChild(textsum);

        getUser(item.userId).then(user => {
            let td6 = tr.insertCell(4);
            let textuserName
            textuserName = document.createTextNode(user.name)
            td6.appendChild(textuserName);

            let td4 = tr.insertCell(5);
            td4.appendChild(editButton);

            let td5 = tr.insertCell(6);
            td5.appendChild(deleteButton);
        });
    });

    gifts = data;
}

const editMyUser = () => {
    document.getElementById('edit-user-name').value = currentUser.name;
    document.getElementById('edit-user-id').innerHTML = currentUser.id;
    document.getElementById('edit-password').value = currentUser.password;
    document.getElementById('edit-email').value = currentUser.email;
    document.getElementById('editUser').style.display = 'block';
}

const updateUser = () => {
    currentUser.name = document.getElementById('edit-user-name').value.trim();
    currentUser.password = document.getElementById('edit-password').value.trim();
    currentUser.email = document.getElementById('edit-email').value.trim();

    fetch(`/user/${currentUser.id}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(currentUser)
    })
        .then((response) => {
            const divedituser = document.getElementById("editUser");
            if (response.status == 400) {
                if (divedituser.lastChild.tagName != "P") {
                    let pError = document.createElement("p");
                    pError.innerHTML = 'invalid user'
                    pError.style = "color: red;";
                    divedituser.appendChild(pError);
                }
            }
            else
                closeEditUser();
        })
        .catch(error => console.error('Unable to update item.', error));

    return false;
}

const closeEditUser = () => {
    document.getElementById('editUser').style.display = 'none';
}

const logout = () => {
    localStorage.removeItem("token");
    window.location.href = "./login.html";
}