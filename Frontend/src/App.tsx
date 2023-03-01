import './App.css';
import React from 'react';
import { Route, Routes } from 'react-router-dom';
import AppRoutes from './AppRoutes';
import { Layout } from './components/Layout';

function App() {
  return (
      <Layout>
        <Routes>
          {AppRoutes.map((route: { [x: string]: any; element: any; }, index: React.Key | null | undefined) => {
            const { element, ...rest } = route;
            return <Route key={index} {...rest} element={element} />;
          })}
        </Routes>
      </Layout>
  );
}

export default App;

