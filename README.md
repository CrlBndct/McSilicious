total = 0
num_items = int(input("How many different items? "))

for i in range(num_items):
    name = input(f"Item {i + 1} name: ")
    price = float(input("Price: "))
    qty = int(input("Quantity: "))
    line_total = price * qty
    total = total + line_total
    print(f"{name}: {qty} x {price:.2f} = {line_total:.2f}")

print(f"\nTotal: {total:.2f}")

# Payment (a for loop lets them retry if the amount is too low)
paid = 0
for attempt in range(3):
    paid = float(input("Enter payment: "))
    if paid >= total:
        break
    print("Not enough money. Try again.")

if paid >= total:
    print(f"Change: {paid - total:.2f}")
    print("Thank you!")
else:
    print("Payment failed. Transaction cancelled.")
