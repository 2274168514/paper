# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a research repository for social media analysis and public opinion monitoring, specifically focusing on political content classification and sentiment analysis. The project appears to be related to reproducing or implementing research from the paper "KaramouzasMademlis_PublicOpinionMonitoringTwitter_SNAM".

## Repository Structure

### Data Organization
- `data/raw/` - Contains multiple datasets for social media analysis:
  - `PSMP/` - Political Social Media Posts dataset (large CSV file)
  - `S19-T6/` - OLID (Offensive Language Identification Dataset) training and test data in TSV format
  - `TSI/` - Twitter Sarcasm/Irony dataset with figurative language classification
  - `archive 2016/` and `archive 2020/` - Historical tweet datasets including hashtag-specific collections
  - `youtube_content/` - YouTube comment data for analysis
- `data/processed/` - Directory for processed/cleaned datasets (currently empty)
- `data/output/` - Directory for analysis results and model outputs (currently empty)
- `notebooks/` - Directory for Jupyter notebooks (currently empty)

### Dataset Characteristics
- **OLID Dataset**: Contains offensive language classification with subtasks (OFF/NOT, TIN/UNT, IND/GRP/OTH)
- **TSI Dataset**: Focuses on figurative language detection in tweets
- **Political datasets**: Include hashtag-based collections for political figures and general political content
- **YouTube data**: Contains comment text for sentiment/content analysis

## Development Context

This repository currently contains only datasets and documentation. When implementing analysis code:

1. **Data Processing**: All datasets are in CSV/TSV format and may require preprocessing for text analysis
2. **Text Classification**: Multiple classification tasks are represented (offensive language, sarcasm, political sentiment)
3. **Research Reproducibility**: Implementation should aim to reproduce results from the referenced academic paper

## Data Handling Notes

- Large datasets may require chunked processing (PSMP dataset exceeds token limits)
- Text data contains social media content with hashtags, mentions, and URLs that may need cleaning
- Multiple classification schemes are present across different datasets
- Some datasets contain potentially sensitive political content for research purposes

## Implementation Progress

### Completed Components
- **Data Analysis**: `src/simple_analysis.py` - OLID dataset structure analysis
- **Data Preprocessing**: `src/data_preprocessing.py` - Text cleaning and dataset preparation
- **Offensive Detection Model**: `src/offensive_detection_model.py` - First implementation of offensive language detection
- **Requirements**: `requirements.txt` - Python dependencies specification

### Dataset Processing Status
- **OLID Dataset**: Fully processed (13,240 training samples, 860 test samples)
  - Subtask A: 13,240 samples (66.77% NOT, 33.23% OFF)
  - Subtask B: 4,400 samples (88.09% TIN, 11.91% UNT)  
  - Subtask C: 3,876 samples (62.10% IND, 27.71% GRP, 10.19% OTH)

### Model Implementation Notes
Current implementation uses simplified features due to environment constraints. For production reproduction:
- Use transformers library with BERT/RoBERTa models
- Implement proper attention mechanisms
- Add advanced NLP preprocessing (proper tokenization, embeddings)

### Next Steps for Full Reproduction
1. Implement remaining three dimensions: polarity, bias, figurativeness
2. Develop aggregation strategies for collective analysis
3. Create prediction pipeline for public opinion monitoring
4. Validate against paper's reported metrics