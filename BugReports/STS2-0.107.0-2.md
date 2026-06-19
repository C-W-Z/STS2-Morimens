# Suggestion/Bug: Generalize Pet/Minion checks to prevent `NullReferenceException` for custom characters.

## Issue

Currently, several powers (e.g., `PersonalHivePower`, `SandpitPower`) hardcode checking for `dealer.Monster is Osty` instead of leveraging the existing PetOwner / Pets architecture.

## Impact

This causes critical crashes. For instance, in `PersonalHivePower.AfterDamageReceived`, if a custom non-Osty minion triggers the damage, dealer remains unassigned to the player, leading to a `NullReferenceException` when calling `CreateCard<Dazed>(dealer.Player)`.

## Suggested Fix

Replace `if (dealer.Monster is Osty)` with a generic check like `if (dealer.IsPet)`, making the core combat engine fully extensible for custom minions.
