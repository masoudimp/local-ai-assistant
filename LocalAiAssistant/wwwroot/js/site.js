const chatBox = document.getElementById("chat-box");
const chatContainer = document.getElementById("chat-container");
const input = document.getElementById("user-input");
const sendBtn = document.getElementById("send-btn");
const attachBtn = document.getElementById("attach-btn");
const fileInput = document.getElementById("file-input");

attachBtn.onclick = () => fileInput.click();

let attachedFile = null;

fileInput.addEventListener("change", () => {
    attachedFile = fileInput.files[0];
    if (attachedFile) {
        addMessage("user", `📎 Attached: ${attachedFile.name}`);
    }
});

async function sendMessage() {
    const text = input.value.trim();
    if (!text && !attachedFile) return;

    // Display user's message
    if (text) addMessage("user", text);
    input.value = "";
    input.style.height = 'auto';

    addMessage("bot", "Thinking... 🤔");

    // Prepare form data (matches [FromForm] ChatMessageRequestDto)
    const formData = new FormData();
    if (text) formData.append("Text", text);
    if (attachedFile) formData.append("File", attachedFile);

    try {
        const res = await fetch("/api/chat/send", {
            method: "POST",
            body: formData
        });

        if (!res.ok) {
            throw new Error(`Server error: ${res.status}`);
        }

        const data = await res.json();

        if (data.success) {
            updateLastMessage(data.response);
        } else {
            updateLastMessage(`❌ Error: ${data.error || "Unknown error"}`);
        }
    } catch (err) {
        updateLastMessage("❌ Failed to connect to server!");
        console.error(err);
    } finally {
        attachedFile = null;
        fileInput.value = ""; // reset file input
    }
}

function addMessage(sender, text) {
    const msgDiv = document.createElement("div");
    msgDiv.className = `msg ${sender}`;

    const avatar = document.createElement("div");
    avatar.className = "avatar";
    avatar.textContent = sender === "user" ? "👤" : "🤖";

    const contentDiv = document.createElement("div");
    contentDiv.className = "msg-content";

    const textDiv = document.createElement("div");
    textDiv.className = "msg-text";
    textDiv.textContent = text;

    contentDiv.appendChild(textDiv);
    msgDiv.appendChild(avatar);
    msgDiv.appendChild(contentDiv);
    chatBox.appendChild(msgDiv);

    chatContainer.scrollTop = chatContainer.scrollHeight;
}

function updateLastMessage(fullText) {
    const msgs = chatBox.getElementsByClassName("bot");
    const last = msgs[msgs.length - 1];
    if (!last) return;

    const textDiv = last.querySelector(".msg-text");
    if (!textDiv) return;

    textDiv.textContent = ""; // clear "Thinking..."
    let index = 0;

    // Create typing animation
    const typingSpeed = 25; // milliseconds per character

    const typeInterval = setInterval(() => {
        if (index < fullText.length) {
            textDiv.textContent += fullText[index];
            chatContainer.scrollTop = chatContainer.scrollHeight;
            index++;
        } else {
            clearInterval(typeInterval);
        }
    }, typingSpeed);
}

input.addEventListener('input', function () {
    this.style.height = 'auto';
    this.style.height = Math.min(this.scrollHeight, 200) + 'px';
});

sendBtn.onclick = sendMessage;

input.addEventListener("keypress", e => {
    if (e.key === "Enter" && !e.shiftKey) {
        e.preventDefault();
        sendMessage();
    }
});