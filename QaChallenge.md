# Quality Advocate – Technical Take-Home Challenge

**Estimated time:** 45–75 minutes
**Submission:** A link to a public GitHub repo (or zip file)

---

## Overview

You'll write a small Playwright test suite against a real, publicly available web application. We're not looking for perfect coverage — we want to see how you think about test design, structure your code, and communicate your decisions.

---

## The App

Use the demo todo app at **https://demo.playwright.dev/todomvc** (the official Playwright sample target — no account needed).

---

## What We're Asking For

### 1. Automated Tests (required)

Write a Playwright test suite in TypeScript that covers the following scenarios:

- Adding a new todo item
- Marking a todo item as complete
- Deleting a todo item
- Filtering todos by "Active" and "Completed"

Your suite should be runnable with a single command (`npx playwright test`).

### 2. One "Edge Case" Test of Your Choice (required)

Add one additional test that covers a scenario you think is worth validating — something a developer might not have thought to test. Briefly comment in the code explaining why you chose it.

### 3. Short Write-Up (required)

Include a `NOTES.md` file (keep it under one page) that answers:

- What would you add or improve if you had another hour?
- How would you integrate this suite into a CI/CD pipeline?
- Is there anything about this app's behavior that surprised or concerned you from a quality standpoint?

---

## Bonus (optional, not required)

- Use the Playwright Page Object Model pattern for any page interactions

---

## What We'll Evaluate

| Area | What we're looking for |
|---|---|
| Test quality | Are assertions meaningful? Do tests fail for the right reasons? |
| Code structure | Is the suite readable and maintainable? |
| Judgment | Did they pick a good edge case? Does the write-up show QA instincts? |
| Communication | Is the `NOTES.md` clear and direct? |

We are **not** grading on how many tests you write. A small, well-structured suite beats a large, brittle one every time.

