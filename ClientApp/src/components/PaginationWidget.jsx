

export function PaginationWidget({ onSelectedPage, onNextPage, onPrevPage, currentPage, pageCount }) {
    console.log("🚀 ~ file: Pagination.jsx:4 ~ Pagination ~ currentPage:", currentPage)

    const PageSelection = () => {
        const arr = [];
        for (let i = 0; i < 5; i++) {
            if (currentPage > 0 && currentPage <= 3) {
                arr.push(i + 1)
            } else if (currentPage > 3) {
                arr.push(currentPage - 2 + i)
            }
        }
        const activeItem = (num) => currentPage == num ? "page-item active" : "page-item"
        return arr.map((el, idx) =>
            <li
                key={idx}
                className={activeItem(el)}>
                <button
                    onClick={() => onSelectedPage(el)}
                    className="page-link"
                    href="#">{el}</button>
            </li>
        )
    }

    return (
        <nav aria-label="Page navigation example">
            <ul className="pagination">
                <li className="page-item">
                    <button id="prevPage" onClick={onPrevPage} className="page-link" href="#" aria-label="Previous">
                        <span aria-hidden="true">&laquo;</span>
                    </button>
                </li>

                <PageSelection />

                <li className="page-item">
                    <button id="nextPage" onClick={onNextPage} className="page-link" href="#" aria-label="Next">
                        <span aria-hidden="true">&raquo;</span>
                    </button>
                </li>
            </ul>
        </nav>
    )
}