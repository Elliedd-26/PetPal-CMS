
document.getElementById("petForm").addEventListener("submit", async function (e) {
  e.preventDefault();

  const petData = {
    name: document.getElementById("petName").value,
    species: document.getElementById("species").value,
    breed: document.getElementById("breed").value,
    birthdate: document.getElementById("birthdate").value
  };

  try {
    const response = await fetch("http://localhost:5112/api/pet", {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(petData)
    });

    if (response.ok) {
      alert("Pet added successfully!");
    } else {
      alert("Error adding pet");
    }
  } catch (err) {
    alert("Network error: " + err.message);
  }
});