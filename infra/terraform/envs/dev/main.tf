# For Day 1, just prove the wiring. We’ll add real resources later.

resource "null_resource" "bootstrap" {
  triggers = {
    note = "day1-skeleton"
  }
}

output "bootstrap_note" {
  value = null_resource.bootstrap.triggers.note
}
