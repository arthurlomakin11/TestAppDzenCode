import { Home } from "./components/Home/Home";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/page/:pageNumber',
    element: <Home />
  }
];

export default AppRoutes;
