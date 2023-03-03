import { Home } from "./components/Home/Home";
import {CommentPage} from "./components/CommentPage/CommentPage";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/page/:pageNumber',
    element: <Home />
  },
  {
    path: '/comment/:commentId',
    element: <CommentPage />
  }
];

export default AppRoutes;
