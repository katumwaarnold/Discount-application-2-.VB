# AI-assisted discount handler proposal (incomplete; review before use)

**AI-assisted work.** This proposal is incomplete without the actual form and project; it has not been verified as a working application.

This is an **additive proposal**, not a complete application or a verified replacement for a real form. It was prepared against `katumwaarnold/Discount-application-2-.VB`, `main` commit `f30f3571c32ec05ce66aef6e9a1e8616fbd5a59a`. That commit has only `README.md` and the extensionless snippet `Final application for discounts based on final amount`; it does not include a `.sln`, `.vbproj`, form or designer. The existing snippet and repository README are deliberately unchanged. The proposed handler matches the snippet's handler/control names, but its applicability to the missing form, event wiring, VB/.NET version and other project code is unverified.

## Behavior changes and assumptions requiring approval

* The text input is the pre-discount price and a percentage. The existing README mentions a discount based on the final amount, so confirm what that means before adoption.
* Uses `Decimal` instead of `Double`; clears both result labels before every attempt; validates nonnegative price and a 0–100 percent inclusive.
* Rounds price to two decimal places first, then rounds the discount to two places using midpoint-away-from-zero; subtracts the rounded discount from the rounded price. These are assumed business rules, not requirements established by the repository. For example, price 1.005 at 50% becomes a discount of 0.51 and a final amount of 0.50. Confirm the rounding order and policy, including currency-specific precision.
* Formats as `C2` using the machine's current culture, including its currency symbol; it does not set or determine a specific currency or enforce a Uganda-specific format. Parsing uses the current culture's decimal separator, accepts leading/trailing whitespace and a leading sign, but deliberately rejects grouping separators and exponents. Output grouping may therefore not be accepted as input.
* Each input may contain at most 28 ASCII digits (including leading/trailing zeros); price must not exceed 999999999999999999.99; percentages must have at most four significant fractional places (extra trailing fractional zeros are accepted). These restrictions are design choices, not verified business rules.

## Integration

If approved and the real project is obtained, replace the existing method **inside its form class**, not the whole form or designer. Ensure `btnCalculateDiscount`, `txtPrice`, `txtDiscount`, `lblDiscountAmount`, and `lblFinalPrice` exist and have the expected types, and ensure the button event is attached exactly once (do not duplicate `Handles` and manual registration). Rebuild and test the actual application, UI, currency/culture and target framework. Do not paste this snippet into a form in addition to the existing handler: that would duplicate the method/event.

## Reproducible tests

Requires Python 3 and the .NET 8 SDK. From this directory, run `python3 tests/run-tests.py` (or set `DOTNET` to the dotnet executable). The runner injects the exact proposal into `tests/Program.template` in a temporary directory and compiles it with `Option Strict On`, without changing the proposal. It uses fake button, labels, text boxes and message box, not Windows Forms. The 22 cases cover ordinary and boundary calculations, rounding, invalid and stale outputs, precision limits, large numbers, and en-US/de-DE parsing/formatting. The tests validate these assumptions, not the intended business rules; they cannot establish real application integration or UI behavior.

Recorded local simulation: .NET SDK 8.0.428, `DOTNET=/home/daytona/workspace/.dotnet/dotnet python3 tests/run-tests.py`: **22 passed, 0 failed**. This was a simulated-control test, not a test of the real Windows Forms application.

## Branch and upload safety

Add this `discount-handler-proposal/` folder on a **new branch** created from the verified base commit, leaving both existing root files untouched. First verify the current repository HEAD and that `discount-handler-proposal/` does not already exist; do not overwrite an existing branch or files. If the branch already exists or main has moved, stop and inspect/rebase manually. A branch proposal is not an approved deployment; review it before merging. No remote write was performed in preparation.
