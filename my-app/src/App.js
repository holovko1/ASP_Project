import './App.css';
import HomePage from "./components/home";
import {Route, Routes} from "react-router-dom";
import CategoryCreatePage from "./components/category/create";
import ShowProducts from "./components/product";

function App() {
    return (
        <>
            <Routes>
                <Route path="/">
                    <Route path={"category"} index element={<HomePage/>} />
                    <Route path={"create"} element={<CategoryCreatePage/>} />
                    <Route path={"product"} element={<ShowProducts/>} />
                </Route>
            </Routes>
        </>
    );
}

export default App;
