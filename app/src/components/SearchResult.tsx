interface Props {
  title: string;
  lyrics: string;
}

const SearchResult = ({ title, lyrics }: Props) => {
  return (
    <div className="search-result-container">
      <span className="search-result-title">{title}</span>
      <span className="search-result-lyrics" dangerouslySetInnerHTML={{ __html: lyrics }} />
    </div>
  );
};

export default SearchResult;
