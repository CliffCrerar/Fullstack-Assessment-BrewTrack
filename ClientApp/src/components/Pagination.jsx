

export function Pagination({next,prev, currentPage, pageCount}) {
    var iterator = new Array(pageCount);
    const onNextPrevClick = (ev) => {
        const btnId = ev.target.id;
        if(btnId === "prevPage") {
            prev();
        }

        if(btnId==="nextPage") {
            next();
        }
    }

    return (
        <nav aria-label="Page navigation example">
            <ul className="pagination">
                <li className="page-item">
                    <button id="prevPage" onClick={onNextPrevClick} className="page-link" href="#" aria-label="Previous">
                        <span aria-hidden="true">&laquo;</span>
                    </button>
                </li>

                <li className="page-item">
                    <button className="page-link" href="#">1</button>
                </li>

                <li className="page-item">
                    <button className="page-link" href="#">2</button>
                </li>

                <li className="page-item">
                    <button className="page-link" href="#">3</button>
                </li>

                <li className="page-item">
                    <button id="nextPage" onClick={onNextPrevClick} className="page-link" href="#" aria-label="Next">
                        <span aria-hidden="true">&raquo;</span>
                    </button>
                </li>
            </ul>
        </nav>
    )
}