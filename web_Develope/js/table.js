//新增数据
function addRow() {
    const table = document.querySelector('table');
    const newRow = document.createElement('tr');
    newRow.innerHTML = `
        <td>Nan</td>
        <td>0987654321</td>
        <td>
            <button onclick="editRow(this)">编辑</button>
            <button onclick="deleteRow(this)">删除</button>
        </td>
    `;
    table.appendChild(newRow);
}

//编辑数据
function editRow(button) {
    const row = button.parentElement.parentElement;
    const name = row.children[0].textContent;
    const contact = row.children[1].textContent;

    // 这里可以弹出一个编辑框，修改数据
    const newName = prompt("修改姓名:", name);
    const newContact = prompt("修改联系方式:", contact);

    if (newName !== null) {
        row.children[0].textContent = newName;
    }
    if (newContact !== null) {
        row.children[1].textContent = newContact;
    }
}

//删除数据
function deleteRow(button) {
    const row = button.parentElement.parentElement;
    if (confirm("确定要删除这行数据吗？")) {
        row.remove();
    }
}