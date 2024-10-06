import { Link, useNavigate } from "react-router-dom";
import { useState, useEffect } from "react"; // Import useEffect here
import axios from "axios";

const ShowProducts = () => {
    const [list, setList] = useState([]);

    useEffect(() => {
        axios.get('http://localhost:5290/api/products')
            .then(res => {
                setList(res.data);
            })
            .catch(error => {
                console.error("There was an error fetching the products!", error);
            });
    }, []);

    console.log("List items", list);

    const handleAddButton = () => {
        setList([
            ...list, {
                id: 2,
                name: "Масло",
                image: "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQXPA1yX3G4SdQRpEDjr56wVaYCPqJwsxsdVg&s"
            }]);
    };

    return (
        <>
            <nav className="navbar navbar-expand-lg navbar-light bg-light">
                <div className="container-fluid">
                    <a className="navbar-brand" href="#">Navbar</a>
                    <button className="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
                        <span className="navbar-toggler-icon"></span>
                    </button>
                    <div className="collapse navbar-collapse" id="navbarNav">
                        <ul className="navbar-nav">
                            <li className="nav-item">
                                <a className="nav-link active" aria-current="page" href="category">Категорії</a>
                            </li>
                            <li className="nav-item">
                                <a className="nav-link" href="product">Продукти</a>
                            </li>
                        </ul>
                    </div>
                </div>
            </nav>
            <div className="container">
                <h1 className="text-center">Список продуктів</h1>
                <Link to={"/create"} className={"btn btn-success"}>Додати</Link>
                {/*<button className={"btn btn-success"} onClick={handleAddButton}>Додати новий</button>*/}
                <table className="table">
                    <thead>
                        <tr>
                            <th scope="col">#</th>
                            <th scope="col">Назва</th>
                            <th scope="col">Ціна</th>
                        </tr>
                    </thead>
                    <tbody>
                        {list.map((item) => (
                            <tr key={item.id}>
                                <th scope="row">{item.id}</th>
                                <td>{item.name}</td>
                                <td>{item.price}</td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
        </>
    );
}

export default ShowProducts; // Change to ShowProducts
