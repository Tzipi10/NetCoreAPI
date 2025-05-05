const uri = '/gift';
let gifts = [];

if(localStorage.getItem("token") == null){
    window.location.href = "./login.html";
}
else{
    // const payload = JSON.parse(atob(token.split('.')[1]));
    // const role=payload["type"]
    // if(role=="admin")
    // {
    //     const usersPageLink=document.getElementById('users');
    //     usersPageLink.style.display = 'block';
    // }
    updateUserId(123); 
}

async function updateUserId(userId) {
    const response = await fetch("/User/updateUserId", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(userId)
    });

    if (response.ok) {
        console.log("UserId updated successfully");
    } else {
        console.error("Failed to update UserId");
    }
}




function getItems() {
    fetch(uri)
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
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(item)
        })
        .then(response => response.json())
        .then(() => {
            getItems();
            addNameTextbox.value = '';
            addPriceTextbox.value = '';
            addSummaryTextbox.value = '';
        })
        .catch(error => console.error('Unable to add item.', error));
}

function deleteItem(id) {
    fetch(`${uri}/${id}`, {
            method: 'DELETE'
        })
        .then(() => getItems())
        .catch(error => console.error('Unable to delete item.', error));
}

function displayEditForm(id) {
    const item = gifts.find(item => item.id === id);

    document.getElementById('edit-name').value = item.name;
    document.getElementById('edit-id').value = item.id;
    document.getElementById('edit-price').value = item.price;
    document.getElementById('edit-summary').value = item.summary;
    document.getElementById('editForm').style.display = 'block';
}

function updateItem() {
    const itemId = document.getElementById('edit-id').value;
    const item = {
        id: parseInt(itemId, 10),
        price: document.getElementById('edit-price').value.trim(),
        name: document.getElementById('edit-name').value.trim(),
        summary: document.getElementById('edit-summary').value.trim()
    };

    fetch(`${uri}/${itemId}`, {
            method: 'PUT',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(item)
        })
        .then(() => getItems())
        .catch(error => console.error('Unable to update item.', error));

    closeInput();

    return false;
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
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
        // let isGlutenFreeCheckbox = document.createElement('input');
        // isGlutenFreeCheckbox.type = 'checkbox';
        // isGlutenFreeCheckbox.disabled = true;
        // isGlutenFreeCheckbox.checked = item.isGlutenFree;

        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${item.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteItem(${item.id})`);

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        let textName = document.createTextNode(item.name);
        td1.appendChild(textName);

        let td2 = tr.insertCell(1);
        let textPrice = document.createTextNode(item.price);
        td2.appendChild(textPrice);

        let td3 = tr.insertCell(2);
        let textsum = document.createTextNode(item.summary);
        td3.appendChild(textsum);

        let td4 = tr.insertCell(3);
        td4.appendChild(editButton);

        let td5 = tr.insertCell(4);
        td5.appendChild(deleteButton);
    });

    gifts = data;
}

const logout=()=>{
    localStorage.removeItem("token");
    window.location.href="./index.html";
}