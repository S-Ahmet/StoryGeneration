/* ============================================================
   🔧 METİN TEMİZLEYİCİ (PDF + EKRAN ORTAK)
============================================================ */
function cleanStoryText(text) {
    return text
        .replace(/_/g, " ")
        .replace(/1/g, "ı")
        .replace(/\s+/g, " ")
        .replace(/\.\s*/g, ".\n\n")
        .trim();
}

/* ============================================================
   📘 HİKAYE OLUŞTURMA
============================================================ */
document.getElementById("storyForm").addEventListener("submit", async (e) => {
    e.preventDefault();

    const button = document.querySelector(".submit-btn");
    button.disabled = true;
    button.textContent = "Oluşturuluyor...";

    const body = {
        tema: document.getElementById("tema").value,
        mekan: document.getElementById("mekan").value,
        zaman: document.getElementById("zaman").value,
        baslangicCumlesi: document.getElementById("baslangic").value,
        ekIstek: document.getElementById("ekIstek").value,
        hedefKelimeSayisi: parseInt(document.getElementById("hedef").value)
    };

    try {
        const res = await fetch("http://localhost:5100/api/story/generate", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(body)
        });

        const data = await res.json();

        if (!data.basarili) {
            alert("❌ Hikaye oluşturulamadı!");
            return;
        }

        const temizMetin = cleanStoryText(data.hikayeMetni);

        document.getElementById("noStory").classList.add("hidden");
        document.getElementById("result").classList.remove("hidden");
        document.getElementById("actionBtns").classList.remove("hidden");

        document.getElementById("baslik").innerText = data.baslik;
        document.getElementById("ozet").innerText = data.ozet;
        document.getElementById("hikaye").innerText = temizMetin;

        saveHistory({
            ...data,
            hikayeMetni: temizMetin
        });

    } catch (err) {
        alert("❌ Backend çalışmıyor!");
    }

    button.disabled = false;
    button.textContent = "Hikaye Oluştur";
});

/* ============================================================
   📋 KOPYALA
============================================================ */
function copyStory() {
    const text = document.getElementById("hikaye").innerText;
    navigator.clipboard.writeText(text);
    alert("📋 Hikaye kopyalandı");
}

/* ============================================================
   📄 PDF — PROFESYONEL FORMAT
============================================================ */
function downloadPDF() {
    const { jsPDF } = window.jspdf;
    const doc = new jsPDF("p", "mm", "a4");

    const title = document.getElementById("baslik").innerText;
    const summary = document.getElementById("ozet").innerText;
    const storyText = cleanStoryText(
        document.getElementById("hikaye").innerText
    );

    doc.setFont("Helvetica", "bold");
    doc.setFontSize(20);
    doc.text(title, 105, 30, { align: "center" });

    doc.setFontSize(11);
    doc.setFont("Helvetica", "normal");
    doc.text(
        "Oluşturulma Tarihi: " + new Date().toLocaleString("tr-TR"),
        105,
        45,
        { align: "center" }
    );

    doc.line(20, 55, 190, 55);

    doc.setFont("Helvetica", "italic");
    doc.setFontSize(11);
    doc.text(summary, 20, 65);

    doc.setFont("Helvetica", "normal");
    doc.setFontSize(12);

    const lines = doc.splitTextToSize(storyText, 170);
    let y = 85;

    lines.forEach(line => {
        if (y > 270) {
            doc.addPage();
            y = 30;
        }
        doc.text(line, 20, y);
        y += 7;
    });

    const pageCount = doc.getNumberOfPages();
    for (let i = 1; i <= pageCount; i++) {
        doc.setPage(i);
        doc.setFontSize(9);
        doc.text(`Sayfa ${i} / ${pageCount}`, 105, 290, { align: "center" });
    }

    doc.save(title.replace(/\s+/g, "_") + ".pdf");
}

/* ============================================================
   🔊 SESLİ OKUMA
============================================================ */
function readStory() {
    const text = document.getElementById("hikaye").innerText;
    if (!text) return;

    const utter = new SpeechSynthesisUtterance(text);
    utter.lang = "tr-TR";
    utter.rate = 1;
    speechSynthesis.speak(utter);
}

/* ============================================================
   💾 GEÇMİŞ KAYDET
============================================================ */
function saveHistory(data) {
    let list = JSON.parse(localStorage.getItem("stories") || "[]");

    list.unshift({
        baslik: data.baslik,
        ozet: data.ozet,
        hikaye: data.hikayeMetni,
        tarih: new Date().toLocaleString()
    });

    localStorage.setItem("stories", JSON.stringify(list));
    loadHistory();
}

/* ============================================================
   📂 GEÇMİŞİ YÜKLE
============================================================ */
function loadHistory() {
    const container = document.getElementById("history");
    container.innerHTML = "";

    let list = JSON.parse(localStorage.getItem("stories") || "[]");

    if (list.length === 0) {
        container.innerHTML = "<p class='no-data'>Henüz geçmiş yok.</p>";
        return;
    }

    list.forEach((item, i) => {
        const div = document.createElement("div");
        div.className = "history-item";

        div.innerHTML = `
            <p><b>${i + 1}) ${item.baslik}</b></p>
            <p class="ozet">${item.ozet}</p>

            <button class="mini-btn" onclick="toggleHistory(${i})">⬇ Aç/Kapa</button>
            <button class="mini-btn" onclick="openModal(${i})">🔍 Detay</button>

            <div id="hist_${i}" class="hidden hikaye-box" style="margin-top:10px;">
                ${item.hikaye}
            </div>
            <hr>
        `;
        container.appendChild(div);
    });
}

loadHistory();

/* ============================================================
   🗑 GEÇMİŞ TEMİZLE
============================================================ */
function clearHistory() {
    if (confirm("Tüm geçmiş silinsin mi?")) {
        localStorage.removeItem("stories");
        loadHistory();
    }
}

/* ============================================================
   ⬇ AÇ / KAPA
============================================================ */
function toggleHistory(i) {
    document.getElementById("hist_" + i).classList.toggle("hidden");
}

/* ============================================================
   🔍 MODAL
============================================================ */
function openModal(i) {
    let list = JSON.parse(localStorage.getItem("stories") || "[]");
    document.getElementById("modalTitle").innerText = list[i].baslik;
    document.getElementById("modalContent").innerText = list[i].hikaye;
    document.getElementById("modal").style.display = "flex";
}

function closeModal() {
    document.getElementById("modal").style.display = "none";
}
