import SearchResult from "./SearchResult";

const Search = () => {
  return (
    <>
      <span className="section-header">Search</span>
      <div className="section-container">
        <div id="search-box-container">
          <textarea name="search-box" id="search-box" />
        </div>
        <div id="search-results-container"></div>
      </div>
    </>
  );
};

export default Search;
