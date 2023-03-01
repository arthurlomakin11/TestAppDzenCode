import 'bootstrap/dist/css/bootstrap.css';
import React from 'react';
import { createRoot } from 'react-dom/client';
import { BrowserRouter } from 'react-router-dom';
import App from './App';

const baseUrl:any = document.getElementsByTagName('base')[0].getAttribute('href');
const root = createRoot(
    document.getElementById('root') as HTMLElement
);

root.render(
    <BrowserRouter basename={baseUrl}>
        <App />
    </BrowserRouter>);