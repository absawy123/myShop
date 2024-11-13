// Function to fetch and display the cart count
function updateCartCount() {
    fetch('/Cart/CartItemCount') // Adjust if your route is different
        .then(response => response.json())
        .then(data => {
            document.getElementById('cart-count').textContent = data.count;
        })
        .catch(error => console.error('Error fetching cart count:', error));
}

// Call the function on page load
document.addEventListener('DOMContentLoaded', function () {
    updateCartCount();
});

