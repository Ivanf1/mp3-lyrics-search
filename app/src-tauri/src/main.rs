#![cfg_attr(
    all(not(debug_assertions), target_os = "windows"),
    windows_subsystem = "windows"
)]

use sysinfo::{System, SystemExt};

// Learn more about Tauri commands at https://tauri.app/v1/guides/features/command
#[tauri::command]
fn check_service(service_name: &str) -> bool {
    let mut sys = System::new_all();
    sys.refresh_all();
    sys.processes_by_exact_name(service_name).count() > 0
}

fn main() {
    tauri::Builder::default()
        .invoke_handler(tauri::generate_handler![check_service])
        .run(tauri::generate_context!())
        .expect("error while running tauri application");
}
