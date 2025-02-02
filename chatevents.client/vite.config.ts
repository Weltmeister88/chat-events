import { fileURLToPath, URL } from 'node:url';
import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-react';
import fs from 'fs';
import path from 'path';

// Determine the base folder based on the OS
const baseFolder =
    process.env.APPDATA !== undefined && process.env.APPDATA !== ''
        ? `${process.env.APPDATA}/ASP.NET/https`
        : `${process.env.HOME}/.aspnet/https`;

const certificateName = "chatevents.client";
const certPath = path.join(baseFolder, `${certificateName}.pfx`);

// Create the directory if it doesn't exist
if (!fs.existsSync(baseFolder)) {
    fs.mkdirSync(baseFolder, { recursive: true });
}

export default defineConfig({
    plugins: [plugin()],
    resolve: {
        alias: {
            '@': fileURLToPath(new URL('./src', import.meta.url))
        }
    },
    server: {
        proxy: {
            '/api': {
                target: 'https://localhost:7008',
                changeOrigin: true,
                secure: false,
            },
        },
        port: 5173,
        https: fs.existsSync(certPath) ? {
            pfx: fs.readFileSync(certPath),
            passphrase: '' // ASP.NET Core dev certs don't have a password
        } : undefined // If no cert exists, Vite will use its own
    }
});