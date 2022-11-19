import SolrService from "./SolrService";

const ServicesStatus = () => {
  return (
    <>
      <span className="section-header">Services status</span>
      <div className="section-container">
        <SolrService />
      </div>
    </>
  );
};

export default ServicesStatus;
